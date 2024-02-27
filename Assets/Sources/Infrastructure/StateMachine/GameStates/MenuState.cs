using System;
using Cysharp.Threading.Tasks;
using Sources.Application.Bootstrapp;
using Sources.ApplicationServicesInterfaces;
using Sources.Controllers.MainMenu;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories.LeaderBoard;
using Sources.InfrastructureInterfaces.Providers;
using Sources.InfrastructureInterfaces.Services;
using Sources.InfrastructureInterfaces.States;
using Sources.Presentation;
using Sources.Presentation.SceneEntity;
using Sources.Presentation.UI;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.Advertisement;
using Sources.Utils.ConstantNames;
using VContainer;

namespace Sources.Infrastructure.StateMachine.GameStates
{
	public sealed class MenuState : IMenuState
	{
		private readonly ISceneLoader _sceneLoader;
		private readonly LoadingCurtain _loadingCurtain;
		private readonly ILevelProgressFacade _levelProgressFacade;
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly ILeaderBoardService _leaderBoardService;
		private readonly ITranslatorService _translatorService;
		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
		private readonly IGameStateChangerProvider _gameStateChangerProvider;
		private readonly CloudServiceSdkFacadeProvider _cloudServiceSdkFacadeProvider;
		private readonly IAuthorizationView _authorizationView;
		private readonly IAssetFactory _assetFactory;

		private MainMenuPresenter _mainMenuPresenter;
		private IAuthorizationPresenter _authorizationPresenter;

		private IGameStateChanger GameStateMachine => _gameStateChangerProvider.Implementation;

		[Inject]
		public MenuState(
			ISceneLoader sceneLoader,
			LoadingCurtain loadingCurtain,
			IAssetFactory assetFactory,
			ILevelProgressFacade levelProgressFacade,
			ILevelConfigGetter levelConfigGetter,
			ILeaderBoardService leaderBoardService,
			IAdvertisement advertisement,
			IAdvertisementHandlerProvider advertisementHandler,
			ITranslatorService translatorService,
			IProgressSaveLoadDataService progressSaveLoadDataService,
			IGameStateChangerProvider gameStateChangerProvider,
			CloudServiceSdkFacadeProvider cloudServiceSdkFacadeProvider
		)
		{
			_levelConfigGetter = levelConfigGetter ?? throw new ArgumentNullException(nameof(levelConfigGetter));
			_leaderBoardService = leaderBoardService ?? throw new ArgumentNullException(nameof(leaderBoardService));

			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));

			_progressSaveLoadDataService = progressSaveLoadDataService ??
				throw new ArgumentNullException(nameof(progressSaveLoadDataService));

			_levelProgressFacade = levelProgressFacade ?? throw new ArgumentNullException(nameof(levelProgressFacade));
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
			_loadingCurtain = loadingCurtain ? loadingCurtain : throw new ArgumentNullException(nameof(loadingCurtain));

			_gameStateChangerProvider = gameStateChangerProvider ??
				throw new ArgumentNullException(nameof(gameStateChangerProvider));
			_cloudServiceSdkFacadeProvider = cloudServiceSdkFacadeProvider ?? throw new ArgumentNullException(nameof(cloudServiceSdkFacadeProvider));
		}

		public async void Enter()
		{
			await _sceneLoader.Load(ConstantNames.MenuScene);

			var authorizationFactory = new AuthorizationFactory(_assetFactory);
			
			_authorizationPresenter = authorizationFactory.Create();
			_authorizationPresenter.Enable();

			await CreateMainMenuPresenter();

			_mainMenuPresenter.Enable();
			_loadingCurtain.HideSlowly();
		}

		public void Exit()
		{
			_authorizationPresenter.Disable();
			_mainMenuPresenter.Disable();
			_loadingCurtain.Show();
			_loadingCurtain.gameObject.SetActive(true);
		}

		private async UniTask<MainMenuPresenter> CreateMainMenuPresenter()
		{
			MainMenuFactory mainMenuFactory = new MainMenuFactory(
				_assetFactory,
				_leaderBoardService,
				_translatorService
			);

			MainMenuBehaviour mainMenuView = await mainMenuFactory.Create();
            
			_mainMenuPresenter = new MainMenuPresenter(
				mainMenuView,
				_levelProgressFacade,
				GameStateMachine,
				_levelConfigGetter,
				_progressSaveLoadDataService,
				_authorizationPresenter
			);
			
			mainMenuView.Construct(_mainMenuPresenter);
			
			return _mainMenuPresenter;
		}
	}
}