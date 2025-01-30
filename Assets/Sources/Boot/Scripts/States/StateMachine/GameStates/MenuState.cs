using System;
using Cysharp.Threading.Tasks;
using Sources.Boot.Scripts.Factories.Presentation.Authorization;
using Sources.Boot.Scripts.Factories.Presentation.LeaderBoard;
using Sources.Boot.Scripts.Factories.Presenters;
using Sources.BusinessLogic;
using Sources.BusinessLogic.Interfaces;
using Sources.BusinessLogic.Interfaces.Factory;
using Sources.BusinessLogic.Services;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.BusinessLogic.States;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.PresentationInterfaces;
using Sources.Utils.ConstantNames;
using VContainer;

namespace Sources.Boot.Scripts.States.StateMachine.GameStates
{
	public sealed class MenuState : IMenuState
	{
		private readonly IAssetLoader _assetLoader;

		private readonly IAuthorizationView _authorizationView;
		private readonly ICloudServiceSdk _cloudServiceSdk;
		private readonly GameFocusHandler _focusHandler;
		private readonly ILeaderBoardPlayersFactory _leaderBoardPlayersFactory;
		private readonly ILeaderBoardService _leaderBoardService;
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly ILevelProgressFacade _levelProgressFacade;
		private readonly ILoadingCurtain _loadingCurtain;
		private readonly ILocalizationService _localizationService;
		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
		private readonly ISceneLoader _sceneLoader;
		private readonly IStateMachine _stateMachine;
		private readonly TranslatorService _translatorService;
		private IAuthorizationPresenter _authorizationPresenter;

		private IMainMenuPresenter _mainMenuPresenter;
		private IMainMenuView _mainMenuView;

		[Inject]
		public MenuState(
			ISceneLoader sceneLoader,
			ILoadingCurtain loadingCurtain,
			IAssetLoader assetLoader,
			ILevelProgressFacade levelProgressFacade,
			ILevelConfigGetter levelConfigGetter,
			ILeaderBoardService leaderBoardService,
			TranslatorService translatorService,
			IProgressSaveLoadDataService progressSaveLoadDataService,
			GameFocusHandler gameFocusHandlerProvider,
			ILocalizationService localizationService,
			ICloudServiceSdk cloudServiceSdk,
			ILeaderBoardPlayersFactory leaderBoardPlayersFactory,
			IStateMachine stateMachine,
			GameFocusHandler focusHandler)
		{
			_sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
			_loadingCurtain = loadingCurtain ?? throw new ArgumentNullException(nameof(loadingCurtain));
			_assetLoader = assetLoader ?? throw new ArgumentNullException(nameof(assetLoader));
			_levelProgressFacade = levelProgressFacade ?? throw new ArgumentNullException(nameof(levelProgressFacade));
			_levelConfigGetter = levelConfigGetter ?? throw new ArgumentNullException(nameof(levelConfigGetter));
			_leaderBoardService = leaderBoardService ?? throw new ArgumentNullException(nameof(leaderBoardService));
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
			_progressSaveLoadDataService = progressSaveLoadDataService
			                               ?? throw new ArgumentNullException(nameof(progressSaveLoadDataService));
			_localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
			_cloudServiceSdk = cloudServiceSdk ?? throw new ArgumentNullException(nameof(cloudServiceSdk));
			_leaderBoardPlayersFactory =
				leaderBoardPlayersFactory ?? throw new ArgumentNullException(nameof(leaderBoardPlayersFactory));

			_stateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
			_focusHandler = focusHandler ?? throw new ArgumentNullException(nameof(focusHandler));
		}

		public void Exit()
		{
			_focusHandler.Disable();
			_authorizationPresenter.Disable();
			_mainMenuPresenter.Disable();
			_loadingCurtain.Show();
		}

		public async UniTask Enter()
		{
			await _sceneLoader.LoadAsync(ConstantNames.MenuScene);
			OnSceneLoaded();
		}

		private IAuthorizationPresenter CreateAuthorizationPresenter() =>
			new AuthorizationFactory(
				_assetLoader,
				_cloudServiceSdk,
				_translatorService
			).Create(_mainMenuView);

		private void EnablePresenters()
		{
			_mainMenuPresenter.Enable();
			_authorizationPresenter.Enable();
		}

		private void InitializeMainMenuPresenter() => _mainMenuView.Construct(_mainMenuPresenter);

		private void OnSceneLoaded()
		{
			_mainMenuView = new MainMenuFactory(_assetLoader, _translatorService).Create();
			_focusHandler.Enable();

#if YANDEX_CODE
			var language = await _cloudServiceSdkFacadeProvider.Self.GetPlayerLanguage();
			Debug.Log("Language: " + language);
			_localizationService.SetLocalLanguage(language);
#endif
			_authorizationPresenter = CreateAuthorizationPresenter();

			_mainMenuPresenter = new MainMenuPresenterFactory(
				_assetLoader,
				_mainMenuView,
				_levelProgressFacade,
				_stateMachine,
				_levelConfigGetter,
				_progressSaveLoadDataService,
				_authorizationPresenter,
				_mainMenuView.LeaderBoardView.GetComponent<ILeaderBoardView>(),
				_leaderBoardService,
				_leaderBoardPlayersFactory,
				_mainMenuView.GetSettingsView()
			).Create();

			InitializeMainMenuPresenter();
			_loadingCurtain.HideSlowly();
			EnablePresenters();
		}
	}
}
