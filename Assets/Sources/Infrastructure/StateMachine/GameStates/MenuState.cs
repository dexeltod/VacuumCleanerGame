using System;
using Cysharp.Threading.Tasks;
using Sources.ApplicationServicesInterfaces;
using Sources.Controllers.MainMenu;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories.Authorization;
using Sources.Infrastructure.Factories.LeaderBoard;
using Sources.Infrastructure.Providers;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Providers;
using Sources.InfrastructureInterfaces.Services;
using Sources.InfrastructureInterfaces.States;
using Sources.Presentation.SceneEntity;
using Sources.Presentation.UI;
using Sources.Presentation.UI.MainMenu.LeaderBoard;
using Sources.PresentationInterfaces;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.Advertisement;
using Sources.Utils.ConstantNames;
using UnityEngine;
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
		private readonly ITranslatorService _localizationService;
		private readonly IAuthorizationView _authorizationView;
		private readonly IAssetFactory _assetFactory;

		private MainMenuPresenter _mainMenuPresenter;
		private IAuthorizationPresenter _authorizationPresenter;
		private MainMenuView _mainMenuView;

		private IGameStateChanger GameStateMachine => _gameStateChangerProvider.Self;

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
			CloudServiceSdkFacadeProvider cloudServiceSdkFacadeProvider,
			ITranslatorService localizationService
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
			_cloudServiceSdkFacadeProvider = cloudServiceSdkFacadeProvider ??
				throw new ArgumentNullException(nameof(cloudServiceSdkFacadeProvider));
			_localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
		}

		public async void Enter()
		{
			Debug.Log("MenuState Enter");
			Debug.Log("_sceneLoader.Load");
			await _sceneLoader.Load(ConstantNames.MenuScene);

			Debug.Log("CreateMainMenuPresenter");
			await CreateMainMenuPresenter();

			Debug.Log("_mainMenuPresenter.Enable");
			_mainMenuPresenter.Enable();
			Debug.Log("_authorizationPresenter.Enable");
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

		private async UniTask<MainMenuPresenter> CreateMainMenuPresenter()
		{
			MainMenuFactory mainMenuFactory = new MainMenuFactory(
				_assetFactory,
				_leaderBoardService,
				_translatorService
			);

			_mainMenuView = await mainMenuFactory.Create();

			_authorizationPresenter = new AuthorizationFactory(
				_assetFactory,
				_cloudServiceSdkFacadeProvider,
				_mainMenuView,
				_localizationService
			).Create();

			_mainMenuPresenter = new MainMenuPresenter(
				_mainMenuView,
				_levelProgressFacade,
				GameStateMachine,
				_levelConfigGetter,
				_progressSaveLoadDataService,
				_authorizationPresenter,
				_mainMenuView.GetComponent<LeaderBoardView>(),
				_leaderBoardService
			);

			_mainMenuView.Construct(_mainMenuPresenter);

			return _mainMenuPresenter;
		}
	}
}