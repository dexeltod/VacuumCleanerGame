using System;
using Sources.BuisenessLogic.Interfaces;
using Sources.BuisenessLogic.Interfaces.Factory;
using Sources.BuisenessLogic.Services;
using Sources.BuisenessLogic.ServicesInterfaces;
using Sources.BuisenessLogic.States;
using Sources.ControllersInterfaces;
using Sources.Domain.Settings;
using Sources.DomainInterfaces;
using Sources.PresentationInterfaces;
using Sources.Utils;
using Sources.Utils.ConstantNames;
using UnityEngine.Audio;
using VContainer;

namespace Sources.BuisenessLogic.StateMachine.GameStates
{
	public sealed class MenuState : IMenuState
	{
		private readonly ILocalizationService _localizationService;
		private readonly ISceneLoader _sceneLoader;
		private readonly ILoadingCurtain _loadingCurtain;
		private readonly ILevelProgressFacade _levelProgressFacade;
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly ILeaderBoardService _leaderBoardService;
		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
		private readonly IGameStateChanger _gameStateChanger;

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
			ISceneLoader sceneLoader,
			ILoadingCurtain loadingCurtain,
			IInjectableAssetFactory injectableAssetFactory,
			IAssetFactory assetFactory,
			ILevelProgressFacade levelProgressFacade,
			ILevelConfigGetter levelConfigGetter,
			ILeaderBoardService leaderBoardService,
			TranslatorService translatorService,
			IProgressSaveLoadDataService progressSaveLoadDataService,
			IGameFocusHandler gameFocusHandlerProvider,
			ILocalizationService localizationService,
			ICloudServiceSdk cloudServiceSdk,
			IGameStateChanger gameStateChanger
		)
		{
			_localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
			_levelConfigGetter = levelConfigGetter ?? throw new ArgumentNullException(nameof(levelConfigGetter));
			_leaderBoardService = leaderBoardService ?? throw new ArgumentNullException(nameof(leaderBoardService));

			_progressSaveLoadDataService = progressSaveLoadDataService ??
			                               throw new ArgumentNullException(nameof(progressSaveLoadDataService));

			_levelProgressFacade = levelProgressFacade ?? throw new ArgumentNullException(nameof(levelProgressFacade));
			_injectableAssetFactory = injectableAssetFactory;
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
			_loadingCurtain = loadingCurtain ?? throw new ArgumentNullException(nameof(loadingCurtain));

			_gameFocusHandler = gameFocusHandlerProvider ??
			                    throw new ArgumentNullException(nameof(gameFocusHandlerProvider));

			_cloudServiceSdk = cloudServiceSdk ?? throw new ArgumentNullException(nameof(cloudServiceSdk));
		}

		public async void Enter()
		{


			_gameFocusHandler.Enable();

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
			MainMenuFactory mainMenuFactory = new MainMenuFactory(
				_injectableAssetFactory,
				_translatorService
			);

			_mainMenuView = mainMenuFactory.Create();

			_authorizationPresenter = new AuthorizationFactory(
				_injectableAssetFactory,
				_cloudServiceSdk,
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
				_gameStateChanger,
				_levelConfigGetter,
				_progressSaveLoadDataService,
				_authorizationPresenter,
				leaderBoardView,
				_leaderBoardService,
				_mainMenuView.GetComponent<SettingsView>(),
				_assetFactory.LoadFromResources<AudioMixer>(ResourcesAssetPath.GameObjects.AudioMixer),
				leaderBoardPlayersFactory,
				new SoundSettings(PlayerPrefs.GetFloat(SettingsPlayerPrefsNames.MasterVolumeName))
			);

			_mainMenuView.Construct(_mainMenuPresenter);
			return _mainMenuPresenter;
		}
	}
}
