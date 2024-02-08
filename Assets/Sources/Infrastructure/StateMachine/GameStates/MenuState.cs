using System;
using Sources.ApplicationServicesInterfaces;
using Sources.Controllers.MainMenu;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories.LeaderBoard;
using Sources.InfrastructureInterfaces.Providers;
using Sources.InfrastructureInterfaces.Services;
using Sources.InfrastructureInterfaces.States.StateMachineInterfaces;
using Sources.Presentation.SceneEntity;
using Sources.Presentation.UI;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.Advertisement;
using Sources.Utils.ConstantNames;
using VContainer;

namespace Sources.Infrastructure.StateMachine.GameStates
{
	public sealed class MenuState : IGameState
	{
		private readonly ISceneLoader _sceneLoader;
		private readonly LoadingCurtain _loadingCurtain;
		private readonly IAssetFactory _assetFactory;
		private readonly ILevelProgressFacade _levelProgressFacade;
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly ILeaderBoardService _leaderBoardService;
		private readonly IRegisterWindowLoader _registerWindowLoader;
		private readonly IAdvertisement _advertisement;
		private readonly ITranslatorService _translatorService;
		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
		private readonly IGameStateChangerProvider _gameStateChangerProvider;

		private MainMenuPresenter _mainMenuPresenter;
		private IGameStateChangerProvider _gameStateMachineProvider;
		private IGameStateChanger GameStateMachine => _gameStateMachineProvider.Instance;

		[Inject]
		public MenuState(
			ISceneLoader sceneLoader,
			LoadingCurtain loadingCurtain,
			IAssetFactory assetFactory,
			ILevelProgressFacade levelProgressFacade,
			ILevelConfigGetter levelConfigGetter,
			ILeaderBoardService leaderBoardService,
			IRegisterWindowLoader registerWindowLoader,
			IAdvertisement advertisement,
			ITranslatorService translatorService,
			IProgressSaveLoadDataService progressSaveLoadDataService,
			IGameStateChangerProvider gameStateChangerProvider
		)
		{
			_levelConfigGetter = levelConfigGetter ?? throw new ArgumentNullException(nameof(levelConfigGetter));
			_leaderBoardService = leaderBoardService ?? throw new ArgumentNullException(nameof(leaderBoardService));

			_registerWindowLoader
				= registerWindowLoader ?? throw new ArgumentNullException(nameof(registerWindowLoader));

			_advertisement = advertisement ?? throw new ArgumentNullException(nameof(advertisement));

			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));

			_progressSaveLoadDataService = progressSaveLoadDataService ??
				throw new ArgumentNullException(nameof(progressSaveLoadDataService));

			_levelProgressFacade = levelProgressFacade ?? throw new ArgumentNullException(nameof(levelProgressFacade));
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
			_loadingCurtain = loadingCurtain ? loadingCurtain : throw new ArgumentNullException(nameof(loadingCurtain));

			_gameStateMachineProvider = gameStateChangerProvider ?? throw new ArgumentNullException(nameof(gameStateChangerProvider));
		}

		public async void Enter()
		{
#if YANDEX_CODE
			YandexGamesSdkFacade yandexGamesSdkFacade = new YandexGamesSdkFacade(_registerWindowLoader.Load());
#endif

			MainMenuFactory mainMenuFactory = new MainMenuFactory(
				_assetFactory,
				_leaderBoardService,
				_translatorService
			);

			await _sceneLoader.Load(ConstantNames.MenuScene);
			await mainMenuFactory.Create();

			MainMenuBehaviour mainMenuBehaviour = mainMenuFactory.MainMenuBehaviour;

			_mainMenuPresenter = new MainMenuPresenter(
				mainMenuBehaviour,
				_levelProgressFacade,
				GameStateMachine,
				_levelConfigGetter,
				_progressSaveLoadDataService
			);

			_mainMenuPresenter.Enable();
			_loadingCurtain.HideSlowly();
		}

		public void Exit()
		{
			_loadingCurtain.Show();
			_loadingCurtain.gameObject.SetActive(true);
			_mainMenuPresenter.Disable();
		}
	}
}