using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sources.Application.StateMachineInterfaces;
using Sources.Application.UnityApplicationServices;
using Sources.ApplicationServicesInterfaces;
using Sources.DIService;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure;
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
using Leaderboard = UnityEngine.SocialPlatforms.Impl.Leaderboard;
#endif

namespace Sources.Application.StateMachine.GameStates
{
	public class InitializeServicesAndProgressState : IGameState
	{
		private readonly GameStateMachine _gameStateMachine;
		private readonly GameServices _gameServices;
		private readonly SceneLoader _sceneLoader;
		private readonly IYandexAuthorizationHandler _yandexAuthorizationHandler;
		private readonly MusicSetter _musicSetter;
		private readonly UnityServicesController _unityServicesController;

		private bool _isServicesRegistered;

		public InitializeServicesAndProgressState
		(
			GameStateMachine gameStateMachine,
			GameServices gameServices,
			SceneLoader sceneLoader,
			IYandexAuthorizationHandler yandexAuthorizationHandler
		)
		{
			_sceneLoader = sceneLoader;
			_yandexAuthorizationHandler = yandexAuthorizationHandler;
			_gameStateMachine = gameStateMachine;
			_gameServices = gameServices;
		}

		public void Exit()
		{
		}

		public async void Enter()
		{
			await RegisterServices();
			await _sceneLoader.Load(ConstantNames.InitialScene);

			await UniTask.WaitWhile(() => _isServicesRegistered == false);
			_gameStateMachine.Enter<InitializeServicesWithProgressState>();
		}

		private async UniTask RegisterServices()
		{
			IAssetProvider provider = _gameServices.Register<IAssetProvider>(new AssetProvider());
			_gameServices.Register<ILocalizationService>(new LocalizationService());

			AbstractLeaderBoard abstractLeaderBoard = new AbstractLeaderBoard(GetLeaderboard());

			LeaderBoardService leaderboard = new LeaderBoardService(abstractLeaderBoard);

			_gameServices.Register<ILeaderBoardService>(leaderboard);

#if YANDEX_GAMES && !UNITY_EDITOR
			YandexGamesSdkFacade yandexSdk =
				new YandexGamesSdkFacade
				(
					_loadingCurtain,
					_yandexAuthorizationHandler
				);

			await yandexSdk.Initialize();

			_gameServices.Register<IYandexSDKController>(yandexSdk);
#endif
			_gameServices.Register<IGameStateMachine>(_gameStateMachine);

			ResourceServiceFactory resourceServiceFactory = new ResourceServiceFactory();
			Dictionary<ResourceType, IResource<int>> intResources = resourceServiceFactory.GetIntResources();
			Dictionary<ResourceType, IResource<float>> floatResources = resourceServiceFactory.GetFloatResources();

			_gameServices.Register<IResourceService>(new ResourcesService(intResources, floatResources));

			IShopItemFactory shopItemFactory = _gameServices.Register<IShopItemFactory>(new ShopItemFactory());

			IPersistentProgressService persistentProgressService =
				_gameServices.Register<IPersistentProgressService>(new PersistentProgressService());

			ISaveLoader saveLoader = await GetSaveLoader
			(
#if YANDEX_GAMES && !UNITY_EDITOR
				yandexSdk,
#endif
				persistentProgressService
			);

			ISaveLoadDataService saveLoadDataService = _gameServices.Register<ISaveLoadDataService>
			(
				new SaveLoadDataService
				(
					saveLoader,
					persistentProgressService
				)
			);

			_gameServices.Register<IUpgradeStatsProvider>(new UpgradeStatsProvider(provider));

			SceneLoadInformer sceneLoadInformer = new SceneLoadInformer();

			_gameServices.Register<IPresenterFactory>(new PresenterFactory());
			_gameServices.Register<ISceneLoadInformer>(sceneLoadInformer);
			_gameServices.Register<ISceneLoad>(sceneLoadInformer);

			CameraFactory cameraFactory = new CameraFactory();
			_gameServices.Register<ICameraFactory>(cameraFactory);
			_gameServices.Register<ICamera>(cameraFactory);
			_gameServices.Register<ISceneConfigGetter>(new SceneConfigGetter());

			ProgressFactory progressFactory = new ProgressFactory
			(
				saveLoadDataService,
				persistentProgressService,
				shopItemFactory,
				saveLoadDataService
			);

			await progressFactory.InitializeProgress();

			_isServicesRegistered = true;
		}

		private async UniTask<ISaveLoader> GetSaveLoader
		(
#if YANDEX_GAMES && !UNITY_EDITOR
			IYandexSDKController sdkController,
#endif
			IPersistentProgressService progressService)
		{
#if YANDEX_GAMES && !UNITY_EDITOR
			return new YandexSaveLoader(sdkController);
#endif

			return await GetEditorSaveLoader(progressService);
		}

		private async UniTask<EditorSaveLoader> GetEditorSaveLoader(IPersistentProgressService progressService)
		{
			IUnityServicesController controller = new UnityServicesController(new InitializationOptions());

			await controller.InitializeUnityServices();

			EditorSaveLoader saveLoader = new EditorSaveLoader(progressService, controller);
			return saveLoader;
		}

		private ILeaderBoard GetLeaderboard()
		{
#if !UNITY_EDITOR && YANDEX_GAMES
			return new YandexLeaderboard();
#endif
			return new EditorLeaderBoardService();
		}
	}

	public class AbstractLeaderBoard : ILeaderBoardInfo
	{
		private readonly ILeaderBoard _leaderboard;

		public int Score { get; }
		private List<int> _scores;

		public AbstractLeaderBoard
		(
			ILeaderBoard leaderboard
		)
		{
			_leaderboard = leaderboard;
		}

		public async UniTask<Dictionary<string, int>> GetPlayers(int playerCount) =>
			await _leaderboard.GetPlayers(playerCount);

		public async UniTask SetScore(int score) =>
			await _leaderboard.Set(score);
	}
}