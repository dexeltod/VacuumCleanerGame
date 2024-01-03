using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sources.Application.Leaderboard;
using Sources.Application.UnityApplicationServices;
using Sources.ApplicationServicesInterfaces;
using Sources.ApplicationServicesInterfaces.StateMachineInterfaces;
using Sources.DIService;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure;
using Sources.Infrastructure.DataViewModel;
using Sources.Infrastructure.Factories;
using Sources.Infrastructure.Factories.UpgradeShop;
using Sources.Infrastructure.Presenters;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Scene;
using Sources.Services;
using Sources.Services.DomainServices;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.Authorization;
using Sources.Utils;
using Sources.Utils.Configs.Scripts;
using Unity.Services.Core;

#if YANDEX_GAMES
using Sources.Services.DomainServices.YandexLeaderboard;
using Agava.YandexGames;
using System.Threading.Tasks;
using Sources.Presentation.UI.YandexAuthorization;
using Sources.PresentationInterfaces;
using Sources.Utils.Configs;
using Sources.Application.YandexSDK;
#endif

namespace Sources.Application.StateMachine.GameStates
{
	public sealed class InitializeServicesAndProgressState : IGameState
	{
		private readonly UnityServicesController _unityServicesController;
		private readonly IYandexAuthorizationView _yandexAuthorizationView;

		private readonly GameStateMachine _gameStateMachine;
		private readonly ServiceLocator _serviceLocator;
		private readonly ISceneLoader _sceneLoader;
		private readonly IAssetProvider _assetProvider;

		private bool _isServicesRegistered;
		private IYandexSDKController _yandexSdkController;

		public InitializeServicesAndProgressState(
			GameStateMachine gameStateMachine,
			ServiceLocator serviceLocator,
			ISceneLoader sceneLoader,
			IAssetProvider assetProvider
		)
		{
			_gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
			_serviceLocator = serviceLocator ?? throw new ArgumentNullException(nameof(serviceLocator));
			_sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
			_assetProvider = assetProvider ?? throw new ArgumentNullException(nameof(assetProvider));
		}

		public void Exit() { }

		public async void Enter()
		{
			await RegisterServices();
			await _sceneLoader.Load(ConstantNames.InitialScene);

			await UniTask.WaitWhile(() => _isServicesRegistered == false);
			_gameStateMachine.Enter<InitializeServicesWithProgressState>();
		}

		private async UniTask RegisterServices()
		{
			_serviceLocator.Register<IGameStateMachine>(_gameStateMachine);
			IAssetProvider assetProvider = _serviceLocator.Register(_assetProvider);
			_serviceLocator.Register<ILocalizationService>(new LocalizationService(assetProvider));

			IAuthorization authorization = new AuthorizationFactory(assetProvider).Create();

			await InitializeLeaderBoardService(authorization);

			IUpgradeDataFactory shopFactory = _serviceLocator.Register<IUpgradeDataFactory>
				(new UpgradeDataFactory(assetProvider));

			IPersistentProgressService persistentProgressService = CreatPersistentProgressService();
			ISaveLoader saveLoader = await GetSaveLoader(
				_yandexSdkController,
				persistentProgressService
			);

			IProgressLoadDataService progressLoadDataService
				= CreateProgressLoadDataService(saveLoader, persistentProgressService);

			IResourceService resourceService = CreateResourceService();

			await CreateProgress(progressLoadDataService, persistentProgressService, shopFactory, resourceService);

			ResourcesProgressPresenter resourcesProgressPresenter = new ResourcesProgressPresenter
				(persistentProgressService.GameProgress.ResourcesModel);

			_serviceLocator.Register<IResourcesProgressPresenter>(resourcesProgressPresenter);

			CreateSceneLoadServices();
			_isServicesRegistered = true;
		}

		private async UniTask CreateProgress(
			IProgressLoadDataService progressLoadDataService,
			IPersistentProgressService persistentProgressService,
			IUpgradeDataFactory upgradeDataFactory,
			IResourceService resourceService
		)
		{
			ProgressFactory progressFactory = CreateProgressFactory(
				progressLoadDataService,
				persistentProgressService,
				upgradeDataFactory,
				resourceService
			);

			await progressFactory.InitializeProgress();
		}

		private void CreateSceneLoadServices()
		{
			ServicesLoadInvokerInformer servicesLoadInvokerInformer = new ServicesLoadInvokerInformer();

			_serviceLocator.Register<ISceneLoadInformer>(servicesLoadInvokerInformer);
			_serviceLocator.Register<ISceneLoadInvoker>(servicesLoadInvokerInformer);
		}

		private ProgressFactory CreateProgressFactory(
			IProgressLoadDataService progressLoadDataService,
			IPersistentProgressService persistentProgressService,
			IUpgradeDataFactory upgradeDataFactory,
			IResourceService resourceService
		)
		{
			ProgressFactory progressFactory = new ProgressFactory(
				progressLoadDataService,
				persistentProgressService,
				upgradeDataFactory,
				progressLoadDataService,
				resourceService
			);

			return progressFactory;
		}

		private IPersistentProgressService CreatPersistentProgressService()
		{
			IPersistentProgressService persistentProgressService =
				_serviceLocator.Register<IPersistentProgressService>
				(
					new PersistentProgressService()
				);

			return persistentProgressService;
		}

		private IProgressLoadDataService CreateProgressLoadDataService(
			ISaveLoader saveLoader,
			IPersistentProgressService persistentProgressService
		)
		{
			IProgressLoadDataService progressLoadDataService =
				_serviceLocator.Register<IProgressLoadDataService>
				(
					new ProgressLoadDataService(saveLoader, persistentProgressService)
				);

			return progressLoadDataService;
		}

		private IResourceService CreateResourceService()
		{
			ResourceServiceFactory resourceServiceFactory = new ResourceServiceFactory();

			Dictionary<ResourceType, IResource<int>> intResources = resourceServiceFactory.GetIntResources();
			Dictionary<ResourceType, IResource<float>> floatResources = resourceServiceFactory.GetFloatResources();

			return _serviceLocator.Register<IResourceService>
			(
				new ResourcesService(
					intResources,
					floatResources
				)
			);
		}

		private async UniTask InitializeLeaderBoardService(IAuthorization handler)
		{
			LeaderBoard leaderBoardService = new LeaderBoard(GetLeaderboard());

			_serviceLocator.Register<ILeaderBoardService>(leaderBoardService);

			YandexGamesSdkFacade yandexSdk = new YandexGamesSdkFacade((IYandexAuthorizationView)handler);

			await yandexSdk.Initialize();

			_serviceLocator.Register<IYandexSDKController>(yandexSdk);
		}

		private async UniTask<ISaveLoader> GetSaveLoader(
			IYandexSDKController sdkController,
			IPersistentProgressService progressService
		)
		{
#if !UNITY_EDITOR
			return new YandexSaveLoader(sdkController);
#endif

#if UNITY_EDITOR
			return await GetEditorSaveLoader(progressService);
#endif
		}

		private async UniTask<EditorSaveLoader> GetEditorSaveLoader(IPersistentProgressService progressService)
		{
			IUnityServicesController controller = new UnityServicesController(new InitializationOptions());

			await controller.InitializeUnityServices();

			EditorSaveLoader saveLoader = new EditorSaveLoader(progressService, controller);
			return saveLoader;
		}

		private IAbstractLeaderBoard GetLeaderboard()
		{
			return new TestLeaderBoardService();

#if !UNITY_EDITOR
			return new YandexLeaderboard();
#endif
		}
	}
}