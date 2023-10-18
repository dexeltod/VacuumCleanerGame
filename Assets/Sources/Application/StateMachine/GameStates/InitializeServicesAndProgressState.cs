using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sources.Application.Leaderboard;
using Sources.Application.StateMachineInterfaces;
using Sources.Application.UnityApplicationServices;
using Sources.ApplicationServicesInterfaces;
using Sources.DIService;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure;
using Sources.Infrastructure.Factories;
using Sources.Infrastructure.Factories.UpgradeShop;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Scene;
using Sources.PresentationInterfaces;
using Sources.Services;
using Sources.Services.DomainServices;
using Sources.Services.Interfaces;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using Sources.Utils;
using Sources.Utils.Configs;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

#if YANDEX_GAMES
using Sources.Services.DomainServices.YandexLeaderboard;
using Agava.YandexGames;
using Sources.Application.YandexSDK;
#endif
using Leaderboard = UnityEngine.SocialPlatforms.Impl.Leaderboard;

namespace Sources.Application.StateMachine.GameStates
{
	public class InitializeServicesAndProgressState : IGameState
	{
		private readonly UnityServicesController _unityServicesController;
#if YANDEX_GAMES && !UNITY_EDITOR
		private readonly IYandexAuthorizationHandler _yandexAuthorizationHandler;
#endif

		private readonly GameStateMachine _gameStateMachine;
		private readonly GameServices     _gameServices;
		private readonly SceneLoader      _sceneLoader;
		private readonly MusicSetter      _musicSetter;

		private bool _isServicesRegistered;

		public InitializeServicesAndProgressState
		(
			IYandexAuthorizationHandler yandexAuthorizationHandler,
			GameStateMachine            gameStateMachine,
			GameServices                gameServices,
			SceneLoader                 sceneLoader
		)
		{
#if YANDEX_GAMES && !UNITY_EDITOR
			_yandexAuthorizationHandler = yandexAuthorizationHandler;
#endif
			_gameStateMachine = gameStateMachine;
			_gameServices     = gameServices;
			_sceneLoader      = sceneLoader;
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
			IGameplayInterfaceView gameplayInterface = _gameServices.Get<IUIGetter>().GameplayInterface;
			SceneLoadService       sceneLoadService  = new SceneLoadService(_gameStateMachine);
			IAssetProvider         assetProvider     = _gameServices.Register<IAssetProvider>(new AssetProvider());

			_gameServices.Register<ILocalizationService>(new LocalizationService());

			CreateLeaderBoard();

			_gameServices.Register<IGameStateMachine>(_gameStateMachine);

			CreateResourceService();

			IShopItemFactory shopItemFactory = _gameServices.Register<IShopItemFactory>(new ShopItemFactory());

			IPersistentProgressService persistentProgressService = CreatPersistentProgressService();

			ILevelProgressPresenter levelProgressPresenter = _gameServices.Register<ILevelProgressPresenter>
			(
				new LevelProgressPresenter
				(
					persistentProgressService.GameProgress.LevelProgress,
					gameplayInterface
				)
			);

			ISaveLoader saveLoader = await GetSaveLoader
									 (
#if YANDEX_GAMES && !UNITY_EDITOR
										 yandexSdk,
#endif
										 persistentProgressService
									 );

			IProgressLoadDataService progressLoadDataService = CreateProgressLoadDataService
				(saveLoader, persistentProgressService);

			_gameServices.Register<IUpgradeStatsProvider>(new UpgradeStatsProvider(assetProvider));

			CreateSceneLoadServices();

			CreateCamera();

			_gameServices.Register<ILevelConfigGetter>(new LevelConfigGetter(assetProvider, levelProgressPresenter));

			ProgressFactory progressFactory = CreateProgressFactory
				(progressLoadDataService, persistentProgressService, shopItemFactory);

			await progressFactory.InitializeProgress();

			_isServicesRegistered = true;
		}

		private void CreateSceneLoadServices()
		{
			SceneLoadInformer sceneLoadInformer = new SceneLoadInformer();

			_gameServices.Register<ISceneLoadInformer>(sceneLoadInformer);
			_gameServices.Register<ISceneLoad>(sceneLoadInformer);
		}

		private static ProgressFactory CreateProgressFactory
		(
			IProgressLoadDataService   progressLoadDataService,
			IPersistentProgressService persistentProgressService,
			IShopItemFactory           shopItemFactory
		)
		{
			ProgressFactory progressFactory = new ProgressFactory
			(
				progressLoadDataService,
				persistentProgressService,
				shopItemFactory,
				progressLoadDataService
			);
			return progressFactory;
		}

		private IPersistentProgressService CreatPersistentProgressService()
		{
			IPersistentProgressService persistentProgressService =
				_gameServices.Register<IPersistentProgressService>
				(
					new PersistentProgressService()
				);
			return persistentProgressService;
		}

		private IProgressLoadDataService CreateProgressLoadDataService
			(ISaveLoader saveLoader, IPersistentProgressService persistentProgressService)
		{
			IProgressLoadDataService progressLoadDataService =
				_gameServices.Register<IProgressLoadDataService>
				(
					new ProgressLoadDataService(saveLoader, persistentProgressService)
				);
			return progressLoadDataService;
		}

		private void CreateCamera()
		{
			CameraFactory cameraFactory = new CameraFactory();
			_gameServices.Register<ICameraFactory>(cameraFactory);
			_gameServices.Register<ICamera>(cameraFactory);
		}

		private void CreateResourceService()
		{
			ResourceServiceFactory resourceServiceFactory = new ResourceServiceFactory();

			Dictionary<ResourceType, IResource<int>>   intResources   = resourceServiceFactory.GetIntResources();
			Dictionary<ResourceType, IResource<float>> floatResources = resourceServiceFactory.GetFloatResources();

			_gameServices.Register<IResourceService>
			(
				new ResourcesService
				(
					intResources,
					floatResources
				)
			);
		}

		private void CreateLeaderBoard()
		{
			LeaderBoard leaderBoardService = new LeaderBoard(GetLeaderboard());

			_gameServices.Register<ILeaderBoardService>(leaderBoardService);

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

	internal class SceneLoadService
	{
		private readonly GameStateMachine _gameStateMachine;

		public SceneLoadService(GameStateMachine gameStateMachine) =>
			_gameStateMachine = gameStateMachine;

		public async UniTask Load(string nextScene)
		{
			if (SceneManager.GetActiveScene().name == nextScene)
				return;

			AsyncOperation waitNextTime = SceneManager.LoadSceneAsync(nextScene);

			await UniTask.WaitWhile(() => waitNextTime.isDone == false);
		}

		public async UniTask Load(LevelConfig config)
		{
			if (SceneManager.GetActiveScene().name == config.LevelName)
				return;

			AsyncOperation waitNextTime = SceneManager.LoadSceneAsync(config.LevelName);

			await UniTask.WaitWhile(() => waitNextTime.isDone == false);
		}
	}
}