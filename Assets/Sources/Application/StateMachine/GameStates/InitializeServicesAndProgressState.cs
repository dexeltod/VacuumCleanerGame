// using System;
// using System.Collections.Generic;
// using Cysharp.Threading.Tasks;
// using Sources.Application.Leaderboard;
// using Sources.Application.UnityApplicationServices;
// using Sources.ApplicationServicesInterfaces;
// using Sources.ApplicationServicesInterfaces.StateMachineInterfaces;
// using Sources.DIService;
// using Sources.DomainInterfaces;
// using Sources.DomainInterfaces.DomainServicesInterfaces;
// using Sources.Infrastructure;
// using Sources.Infrastructure.DataViewModel;
// using Sources.Infrastructure.Factories;
// using Sources.Infrastructure.Factories.UpgradeShop;
// using Sources.Infrastructure.Presenters;
// using Sources.InfrastructureInterfaces.Factory;
// using Sources.InfrastructureInterfaces.Scene;
// using Sources.Services;
// using Sources.Services.DomainServices;
// using Sources.Services.Localization;
// using Sources.ServicesInterfaces;
// using Sources.ServicesInterfaces.Authorization;
// using Sources.Utils;
// using Sources.Utils.Configs.Scripts;
// using Unity.Services.Core;
// using VContainer;
//
// #if YANDEX_GAMES
// using Sources.Services.DomainServices.YandexLeaderboard;
// using Agava.YandexGames;
// using System.Threading.Tasks;
// using Sources.Presentation.UI.YandexAuthorization;
// using Sources.PresentationInterfaces;
// using Sources.Utils.Configs;
// using Sources.Application.YandexSDK;
// #endif
//
// namespace Sources.Application.StateMachine.GameStates
// {
// 	public sealed class InitializeServicesAndProgressState : IGameState
// 	{
// 		private readonly UnityServicesController _unityServicesController;
// 		private readonly IYandexAuthorizationView _yandexAuthorizationView;
//
// 		private readonly GameStateMachine _gameStateMachine;
// 		private readonly IObjectResolver _container;
// 		private readonly ISceneLoader _sceneLoader;
// 		private readonly IContainerBuilder _builder;
//
// 		private bool _isServicesRegistered;
// 		private IYandexSDKController _yandexSdkController;
//
// 		public InitializeServicesAndProgressState(
// 			GameStateMachine gameStateMachine,
// 			IObjectResolver container,
// 			ISceneLoader sceneLoader,
// 			IContainerBuilder containerBuilder
// 		)
// 		{
// 			_gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
// 			_container = container ?? throw new ArgumentNullException(nameof(container));
// 			_sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
// 			_builder = containerBuilder ?? throw new ArgumentNullException(nameof(containerBuilder));
// 		}
//
// 		public void Exit() { }
//
// 		public async void Enter()
// 		{
// 			await RegisterServices();
// 			await _sceneLoader.Load(ConstantNames.InitialScene);
//
// 			await UniTask.WaitWhile(() => _isServicesRegistered == false);
// 			_gameStateMachine.Enter<InitializeServicesWithProgressState>();
// 		}
//
// 		private async UniTask RegisterServices()
// 		{
// 			var assetProvider = _container.Resolve<IAssetProvider>();
// 			_builder.Register<ILocalizationService, LocalizationService>(Lifetime.Singleton)
// 				.WithParameter(_container.Resolve<IAssetProvider>());
//
// 			IAuthorization authorization = new AuthorizationFactory(assetProvider).Create();
//
// 			IRewardService rewardService = new YandexRewardsService();
// 			await InitializeLeaderBoardService(authorization, rewardService);
//
// 			_builder.Register<IUpgradeDataFactory, UpgradeDataFactory>(Lifetime.Scoped);
//
// 			_builder.Register<IPersistentProgressService, PersistentProgressService>(Lifetime.Singleton);
//
// 			var persistentProgressService = _container.Resolve<IPersistentProgressService>();
// 			ISaveLoader saveLoader = await GetSaveLoader(
// 				_yandexSdkController,
// 				persistentProgressService
// 			);
//
// 			IProgressLoadDataService progressLoadDataService
// 				= CreateProgressLoadDataService(saveLoader, persistentProgressService);
//
// 			IResourceService resourceService = CreateResourceService();
//
// 			
// 			ProgressFactory progressFactory = CreateProgressFactory(
// 				progressLoadDataService,
// 				persistentProgressService,
// 				upgradeDataFactory,
// 				resourceService
// 			);
//
// 			await progressFactory.InitializeProgress();
//
// 			await CreateProgress(
// 				progressLoadDataService,
// 				persistentProgressService,
// 				upgradeDataFactory,
// 				resourceService
// 			);
//
// 			ResourcesProgressPresenter resourcesProgressPresenter = new ResourcesProgressPresenter
// 				(persistentProgressService.GameProgress.ResourcesModel);
//
// 			_builder.RegisterInstance<IResourcesProgressPresenter, ResourcesProgressPresenter>(
// 				resourcesProgressPresenter
// 			);
//
// 			CreateSceneLoadServices();
// 			_isServicesRegistered = true;
// 		}
//
// 		private async UniTask CreateProgress(
// 			IProgressLoadDataService progressLoadDataService,
// 			IPersistentProgressService persistentProgressService,
// 			IUpgradeDataFactory upgradeDataFactory,
// 			IResourceService resourceService
// 		) { }
//
// 		private void CreateSceneLoadServices()
// 		{
// 			ServicesLoadInvokerInformer servicesLoadInvokerInformer = new ServicesLoadInvokerInformer();
//
// 			_builder.RegisterInstance<ISceneLoadInformer>(servicesLoadInvokerInformer);
// 			_builder.RegisterInstance<ISceneLoadInvoker>(servicesLoadInvokerInformer);
// 		}
//
// 		private ProgressFactory CreateProgressFactory(
// 			IProgressLoadDataService progressLoadDataService,
// 			IPersistentProgressService persistentProgressService,
// 			IUpgradeDataFactory upgradeDataFactory,
// 			IResourceService resourceService
// 		)
// 		{
// 			ProgressFactory progressFactory = new ProgressFactory(
// 				progressLoadDataService,
// 				persistentProgressService,
// 				upgradeDataFactory,
// 				progressLoadDataService,
// 				resourceService
// 			);
//
// 			return progressFactory;
// 		}
//
// 		private IProgressLoadDataService CreateProgressLoadDataService(
// 			ISaveLoader saveLoader,
// 			IPersistentProgressService persistentProgressService
// 		)
// 		{
// 			_builder.RegisterInstance<IProgressLoadDataService, ProgressLoadDataService>
// 			(
// 				new ProgressLoadDataService(saveLoader, persistentProgressService)
// 			);
//
// 			return _container.Resolve<IProgressLoadDataService>();
// 		}
//
// 		private IResourceService CreateResourceService()
// 		{
// 			ResourceServiceFactory resourceServiceFactory = new ResourceServiceFactory();
//
// 			Dictionary<ResourceType, IResource<int>> intResources = resourceServiceFactory.GetIntResources();
// 			Dictionary<ResourceType, IResource<float>> floatResources = resourceServiceFactory.GetFloatResources();
//
// 			_builder.RegisterInstance<IResourceService>
// 			(
// 				new ResourcesService(
// 					intResources,
// 					floatResources
// 				)
// 			);
//
// 			return _container.Resolve<IResourceService>();
// 		}
//
// 		private async UniTask InitializeLeaderBoardService(IAuthorization handler, IRewardService rewardService)
// 		{
// 			LeaderBoard leaderBoardService = new LeaderBoard(GetLeaderboard());
//
// 			_builder.RegisterInstance<ILeaderBoardService>(leaderBoardService);
//
// #if YANDEX_GAMES && YANDEX_CODE
// 			YandexGamesSdkFacade yandexSdk = new YandexGamesSdkFacade((IYandexAuthorizationView)handler, rewardService);
// 			await yandexSdk.Initialize();
//
// 			_builder.RegisterInstance<IYandexSDKController>(yandexSdk);
// #endif
// 		}
//
// 		private async UniTask<ISaveLoader> GetSaveLoader(
// 			IYandexSDKController sdkController,
// 			IPersistentProgressService progressService
// 		)
// 		{
// #if YANDEX_GAMES && !UNITY_EDITOR
// 			return new YandexSaveLoader(sdkController);
// #endif
//
// #if UNITY_EDITOR
// 			return await GetEditorSaveLoader(progressService);
// #endif
// 		}
//
// 		private async UniTask<EditorSaveLoader> GetEditorSaveLoader(IPersistentProgressService progressService)
// 		{
// 			IUnityServicesController controller = new UnityServicesController(new InitializationOptions());
//
// 			await controller.InitializeUnityServices();
//
// 			EditorSaveLoader saveLoader = new EditorSaveLoader(progressService, controller);
// 			return saveLoader;
// 		}
//
// 		private IAbstractLeaderBoard GetLeaderboard()
// 		{
// 			return new TestLeaderBoardService();
//
// #if !UNITY_EDITOR
// 			return new YandexLeaderboard();
// #endif
// 		}
// 	}
// }