using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Sources.Application.Leaderboard;
using Sources.Application.StateMachineInterfaces;
using Sources.Application.UnityApplicationServices;
using Sources.ApplicationServicesInterfaces;
using Sources.DIService;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure;
using Sources.Infrastructure.DataViewModel;
using Sources.Infrastructure.Factories;
using Sources.Infrastructure.Factories.UpgradeShop;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Scene;
using Sources.PresentationInterfaces;
using Sources.Services;
using Sources.Services.DomainServices;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using Sources.Utils;
using Sources.Utils.Configs;
using Unity.Services.Core;

#if YANDEX_GAMES
using Sources.Services.DomainServices.YandexLeaderboard;
using Agava.YandexGames;
using Sources.Application.YandexSDK;
#endif

namespace Sources.Application.StateMachine.GameStates
{
	public sealed class InitializeServicesAndProgressState : IGameState
	{
		private readonly UnityServicesController _unityServicesController;
#if YANDEX_GAMES && !UNITY_EDITOR
		private readonly IYandexAuthorizationHandler _yandexAuthorizationHandler;
#endif

		private readonly GameStateMachine _gameStateMachine;
		private readonly ServiceLocator _serviceLocator;
		private readonly SceneLoader _sceneLoader;
		private readonly MusicSetter _musicSetter;

		private bool _isServicesRegistered;

		public InitializeServicesAndProgressState
		(
			IYandexAuthorizationHandler yandexAuthorizationHandler,
			GameStateMachine gameStateMachine,
			ServiceLocator serviceLocator,
			SceneLoader sceneLoader
		)
		{
#if YANDEX_GAMES && !UNITY_EDITOR
			_yandexAuthorizationHandler = yandexAuthorizationHandler;
#endif
			_gameStateMachine = gameStateMachine;
			_serviceLocator = serviceLocator;
			_sceneLoader = sceneLoader;
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

			IAssetProvider assetProvider = _serviceLocator.Register<IAssetProvider>(new AssetProvider());
			_serviceLocator.Register<ILocalizationService>(new LocalizationService(assetProvider));

			CreateLeaderBoardService();

			IUpgradeDataFactory shopFactory = _serviceLocator.Register<IUpgradeDataFactory>
				(new UpgradeDataFactory(assetProvider));

			IPersistentProgressService persistentProgressService = CreatPersistentProgressService();
			ISaveLoader saveLoader = await GetSaveLoader
			(
#if YANDEX_GAMES && !UNITY_EDITOR
										 yandexSdk,
#endif
				persistentProgressService
			);

			IProgressLoadDataService progressLoadDataService = CreateProgressLoadDataService
				(saveLoader, persistentProgressService);

			IResourceService resourceService = CreateResourceService();

			await CreateProgress(progressLoadDataService, persistentProgressService, shopFactory, resourceService);

			ResourcesProgressPresenter resourcesProgressPresenter = new ResourcesProgressPresenter
				(persistentProgressService.GameProgress.ResourcesModel);

			_serviceLocator.Register<IResourcesProgressPresenter>(resourcesProgressPresenter);

			CreateSceneLoadServices();
			_isServicesRegistered = true;
		}

		private async UniTask CreateProgress
		(
			IProgressLoadDataService progressLoadDataService,
			IPersistentProgressService persistentProgressService,
			IUpgradeDataFactory upgradeDataFactory,
			IResourceService resourceService
		)
		{
			ProgressFactory progressFactory = CreateProgressFactory
			(
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

		private ProgressFactory CreateProgressFactory
		(
			IProgressLoadDataService progressLoadDataService,
			IPersistentProgressService persistentProgressService,
			IUpgradeDataFactory upgradeDataFactory,
			IResourceService resourceService
		)
		{
			ProgressFactory progressFactory = new ProgressFactory
			(
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

		private IProgressLoadDataService CreateProgressLoadDataService
		(
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
				new ResourcesService
				(
					intResources,
					floatResources
				)
			);
		}

		private void CreateLeaderBoardService()
		{
			LeaderBoard leaderBoardService = new LeaderBoard(GetLeaderboard());

			_serviceLocator.Register<ILeaderBoardService>(leaderBoardService);

#if YANDEX_GAMES && !UNITY_EDITOR
			YandexGamesSdkFacade yandexSdk =
				new YandexGamesSdkFacade(_yandexAuthorizationHandler);

			await yandexSdk.Initialize();

			_gameServices.Register<IYandexSDKController>(yandexSdk);
#endif
		}

		private async UniTask<ISaveLoader> GetSaveLoader
		(
#if YANDEX_GAMES && !UNITY_EDITOR
			IYandexSDKController sdkController,
#endif
			IPersistentProgressService progressService
		)
		{
#if YANDEX_GAMES && !UNITY_EDITOR
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
#if !UNITY_EDITOR && YANDEX_GAMES
			return new YandexLeaderboard();
#endif
#if UNITY_EDITOR
			return new EditorAbstractLeaderBoardService();
#endif
		}
	}
}