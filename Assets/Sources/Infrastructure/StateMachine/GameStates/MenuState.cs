using System;
using Sources.ApplicationServicesInterfaces;
using Sources.Controllers.MainMenu;
using Sources.ControllersInterfaces;
using Sources.Domain.Settings;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories;
using Sources.Infrastructure.Factories.Authorization;
using Sources.Infrastructure.Factories.LeaderBoard;
using Sources.Infrastructure.Providers;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Providers;
using Sources.InfrastructureInterfaces.Services;
using Sources.InfrastructureInterfaces.States;
using Sources.Presentation.SceneEntity;
using Sources.Presentation.UI;
using Sources.Presentation.UI.MainMenu.LeaderBoard;
using Sources.PresentationInterfaces;
using Sources.Services;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using Sources.Utils;
using Sources.Utils.ConstantNames;
using UnityEngine;
using UnityEngine.Audio;
using VContainer;

namespace Sources.Infrastructure.StateMachine.GameStates
{
	public sealed class MenuState : IMenuState
	{
		private readonly ILocalizationService _localizationService;
		private readonly ISceneLoader _sceneLoader;
		private readonly LoadingCurtain _loadingCurtain;
		private readonly ILevelProgressFacade _levelProgressFacade;
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly ILeaderBoardService _leaderBoardService;
		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
		private readonly IGameStateChangerProvider _gameStateChangerProvider;
		private readonly CloudServiceSdkFacadeProvider _cloudServiceSdkFacadeProvider;
		private readonly GameFocusHandlerProvider _gameFocusHandlerProvider;
		private readonly IAuthorizationView _authorizationView;
		private readonly IAssetFactory _injectableAssetFactory;
		private readonly AssetFactory _assetFactory;
		private readonly ITranslatorService _translatorService;

		private IMainMenuPresenter _mainMenuPresenter;
		private IAuthorizationPresenter _authorizationPresenter;
		private MainMenuView _mainMenuView;
		private GameFocusHandler _gameFocusHandler;

		private IGameStateChanger GameStateMachine => _gameStateChangerProvider.Self;

		[Inject]
		public MenuState(
			ISceneLoader sceneLoader,
			LoadingCurtain loadingCurtain,
			InjectableAssetFactory injectableAssetFactory,
			AssetFactory assetFactory,
			ILevelProgressFacade levelProgressFacade,
			ILevelConfigGetter levelConfigGetter,
			ILeaderBoardService leaderBoardService,
			ITranslatorService translatorService,
			IProgressSaveLoadDataService progressSaveLoadDataService,
			IGameStateChangerProvider gameStateChangerProvider,
			CloudServiceSdkFacadeProvider cloudServiceSdkFacadeProvider,
			GameFocusHandlerProvider gameFocusHandlerProvider,
			ILocalizationService localizationService
		)
		{
			_localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
			_levelConfigGetter = levelConfigGetter ?? throw new ArgumentNullException(nameof(levelConfigGetter));
			_leaderBoardService = leaderBoardService ?? throw new ArgumentNullException(nameof(leaderBoardService));

			_progressSaveLoadDataService = progressSaveLoadDataService ??
				throw new ArgumentNullException(nameof(progressSaveLoadDataService));

			_levelProgressFacade = levelProgressFacade ?? throw new ArgumentNullException(nameof(levelProgressFacade));
			_injectableAssetFactory
				= injectableAssetFactory ?? throw new ArgumentNullException(nameof(injectableAssetFactory));
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
			_loadingCurtain = loadingCurtain ? loadingCurtain : throw new ArgumentNullException(nameof(loadingCurtain));

			_gameStateChangerProvider = gameStateChangerProvider ??
				throw new ArgumentNullException(nameof(gameStateChangerProvider));
			_cloudServiceSdkFacadeProvider = cloudServiceSdkFacadeProvider ??
				throw new ArgumentNullException(nameof(cloudServiceSdkFacadeProvider));
			_gameFocusHandlerProvider = gameFocusHandlerProvider ??
				throw new ArgumentNullException(nameof(gameFocusHandlerProvider));
		}

		public async void Enter()
		{
			_gameFocusHandlerProvider.Self.Enable();

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
			_loadingCurtain.gameObject.SetActive(true);
		}

		private IMainMenuPresenter CreateMainMenuPresenter()
		{
			MainMenuFactory mainMenuFactory = new MainMenuFactory(
				_injectableAssetFactory,
				_translatorService
			);

			_mainMenuView = mainMenuFactory.Create();

			_authorizationPresenter = new AuthorizationFactory(
				_injectableAssetFactory,
				_cloudServiceSdkFacadeProvider,
				_mainMenuView,
				_translatorService
			).Create();

			LeaderBoardView leaderBoardView = _mainMenuView.LeaderBoardView.GetComponent<LeaderBoardView>();

			ILeaderBoardPlayersFactory leaderBoardPlayersFactory = new LeaderBoardPlayersFactory(
				_assetFactory,
				leaderBoardView,
				_leaderBoardService,
				_translatorService
			);

			_mainMenuPresenter = new MainMenuPresenter(
				_mainMenuView,
				_levelProgressFacade,
				GameStateMachine,
				_levelConfigGetter,
				_progressSaveLoadDataService,
				_authorizationPresenter,
				leaderBoardView,
				_leaderBoardService,
				_mainMenuView.GetComponent<SettingsView>(),
				_assetFactory.LoadFromResources<AudioMixer>(ResourcesAssetPath.GameObjects.AudioMixer),
				leaderBoardPlayersFactory,
				new SoundSettings(PlayerPrefs.GetFloat(SettingsPlayerPrefsNames.MasterVolumeName)),
				new LeaderFactory(leaderBoardPlayersFactory)
			);

			_mainMenuView.Construct(_mainMenuPresenter);
			return _mainMenuPresenter;
		}
	}
}