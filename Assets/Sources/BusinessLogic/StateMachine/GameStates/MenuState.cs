using System;
using Sources.BusinessLogic.Interfaces;
using Sources.BusinessLogic.Interfaces.Factory;
using Sources.BusinessLogic.Services;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.BusinessLogic.States;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces.Factories.Presentations;
using Sources.InfrastructureInterfaces.Factories.Presenters;
using Sources.PresentationInterfaces;
using Sources.Utils.ConstantNames;
using VContainer;

namespace Sources.BusinessLogic.StateMachine.GameStates
{
	public sealed class MenuState : IMenuState
	{
		private readonly IMainMenuFactory _mainMenuFactory;
		private readonly GameFocusHandler _gameFocusHandlerProvider;
		private readonly ILocalizationService _localizationService;
		private readonly ISceneLoader _sceneLoader;
		private readonly ILoadingCurtain _loadingCurtain;
		private readonly ILevelProgressFacade _levelProgressFacade;
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly ILeaderBoardService _leaderBoardService;
		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
		private readonly IGameStateChanger _gameStateChanger;
		private readonly IAuthorizationFactory _authorizationFactory;
		private readonly ILeaderBoardPlayersFactory _leaderBoardPlayersFactory;
		private readonly IMainMenuPresenterFactory _mainMenuPresenterFactory;

		private readonly GameFocusHandler _gameFocusHandler;
		private readonly IAuthorizationView _authorizationView;
		private readonly IAssetFactory _injectableAssetFactory;
		private readonly IAssetFactory _assetFactory;
		private readonly TranslatorService _translatorService;
		private readonly ICloudServiceSdk _cloudServiceSdk;

		private IMainMenuPresenter _mainMenuPresenter;
		private IAuthorizationPresenter _authorizationPresenter;
		private IMainMenuView _mainMenuView;

		[Inject]
		public MenuState(
			IMainMenuFactory mainMenuFactory,
			ISceneLoader sceneLoader,
			ILoadingCurtain loadingCurtain,
			IInjectableAssetFactory injectableAssetFactory,
			IAssetFactory assetFactory,
			ILevelProgressFacade levelProgressFacade,
			ILevelConfigGetter levelConfigGetter,
			ILeaderBoardService leaderBoardService,
			TranslatorService translatorService,
			IProgressSaveLoadDataService progressSaveLoadDataService,
			GameFocusHandler gameFocusHandlerProvider,
			ILocalizationService localizationService,
			ICloudServiceSdk cloudServiceSdk,
			IGameStateChanger gameStateChanger,
			IAuthorizationFactory authorizationFactory,
			ILeaderBoardPlayersFactory leaderBoardPlayersFactory,
			IMainMenuPresenterFactory mainMenuPresenterFactory
		)
		{
			_mainMenuFactory = mainMenuFactory ?? throw new ArgumentNullException(nameof(mainMenuFactory));
			_sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
			_loadingCurtain = loadingCurtain ?? throw new ArgumentNullException(nameof(loadingCurtain));
			_injectableAssetFactory = injectableAssetFactory ?? throw new ArgumentNullException(nameof(injectableAssetFactory));
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_levelProgressFacade = levelProgressFacade ?? throw new ArgumentNullException(nameof(levelProgressFacade));
			_levelConfigGetter = levelConfigGetter ?? throw new ArgumentNullException(nameof(levelConfigGetter));
			_leaderBoardService = leaderBoardService ?? throw new ArgumentNullException(nameof(leaderBoardService));
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
			_progressSaveLoadDataService = progressSaveLoadDataService ??
			                               throw new ArgumentNullException(nameof(progressSaveLoadDataService));
			_gameFocusHandlerProvider =
				gameFocusHandlerProvider ?? throw new ArgumentNullException(nameof(gameFocusHandlerProvider));
			_localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
			_cloudServiceSdk = cloudServiceSdk ?? throw new ArgumentNullException(nameof(cloudServiceSdk));
			_gameStateChanger = gameStateChanger ?? throw new ArgumentNullException(nameof(gameStateChanger));
			_authorizationFactory = authorizationFactory ?? throw new ArgumentNullException(nameof(authorizationFactory));
			_leaderBoardPlayersFactory =
				leaderBoardPlayersFactory ?? throw new ArgumentNullException(nameof(leaderBoardPlayersFactory));
			_mainMenuPresenterFactory = mainMenuPresenterFactory;
		}

		public async void Enter()
		{
			_gameFocusHandler.Enable();
			_mainMenuView = _mainMenuFactory.Create();
			await _sceneLoader.Load(ConstantNames.MenuScene);

			CreateMainMenuPresenter();

#if YANDEX_CODE
			var language = await _cloudServiceSdkFacadeProvider.Self.GetPlayerLanguage();
			Debug.Log("Language: " + language);
			_localizationService.SetLocalLanguage(language);
#endif

			_mainMenuPresenter.Enable();
			_authorizationPresenter.Enable();

			_loadingCurtain.HideSlowly();
		}

		public void Exit()
		{
			_authorizationPresenter.Disable();
			_mainMenuPresenter.Disable();
			_loadingCurtain.Show();
		}

		private IMainMenuPresenter CreateMainMenuPresenter()
		{
			_authorizationPresenter = _authorizationFactory.Create(_mainMenuView);

			ILeaderBoardView leaderBoardView = _mainMenuView.LeaderBoardView.GetComponent<ILeaderBoardView>();

			_mainMenuPresenter = _mainMenuPresenterFactory.Create(_mainMenuView,)(
			);

			_mainMenuView.Construct(_mainMenuPresenter);
			return _mainMenuPresenter;
		}
	}
}