using System;
using Sources.ApplicationServicesInterfaces;
using Sources.ApplicationServicesInterfaces.StateMachineInterfaces;
using Sources.Controllers;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories.LeaderBoard;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Scene;
using Sources.Presentation;
using Sources.Presentation.Implementation;
using Sources.Presentation.SceneEntity;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.Advertisement;
using Sources.Utils.Configs.Scripts;
using Sources.Utils.ConstantNames;
using VContainer;

namespace Sources.Application.StateMachine.GameStates
{
	public sealed class MenuState : IGameState
	{
		private readonly ISceneLoader _sceneLoader;
		private readonly LoadingCurtain _loadingCurtain;
		private readonly IAssetResolver _assetResolver;
		private readonly ILevelProgressFacade _levelProgressFacade;
		private readonly IGameStateMachine _gameStateMachine;
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly ILeaderBoardService _leaderBoardService;
		private readonly IRegisterWindowLoader _registerWindowLoader;
		private readonly IAdvertisement _advertisement;
		private readonly ITranslatorService _translatorService;
		private readonly IProgressLoadDataService _progressLoadDataService;

		private MainMenuPresenter _mainMenuPresenter;

		[Inject]
		public MenuState(
			ISceneLoader sceneLoader,
			LoadingCurtain loadingCurtain,
			IAssetResolver assetResolver,
			ILevelProgressFacade levelProgressFacade,
			IGameStateMachine gameStateMachine,
			ILevelConfigGetter levelConfigGetter,
			ILeaderBoardService leaderBoardService,
			IRegisterWindowLoader registerWindowLoader,
			IAdvertisement advertisement,
			ITranslatorService translatorService,
			IProgressLoadDataService progressLoadDataService
		)
		{
			_levelConfigGetter = levelConfigGetter ?? throw new ArgumentNullException(nameof(levelConfigGetter));
			_leaderBoardService = leaderBoardService ?? throw new ArgumentNullException(nameof(leaderBoardService));

			_registerWindowLoader
				= registerWindowLoader ?? throw new ArgumentNullException(nameof(registerWindowLoader));

			_advertisement = advertisement ?? throw new ArgumentNullException(nameof(advertisement));
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
			_progressLoadDataService = progressLoadDataService ?? throw new ArgumentNullException(nameof(progressLoadDataService));
			_gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
			_levelProgressFacade = levelProgressFacade ?? throw new ArgumentNullException(nameof(levelProgressFacade));
			_assetResolver = assetResolver ?? throw new ArgumentNullException(nameof(assetResolver));
			_sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
			_loadingCurtain = loadingCurtain ? loadingCurtain : throw new ArgumentNullException(nameof(loadingCurtain));
		}

		public async void Enter()
		{
#if YANDEX_CODE
			YandexGamesSdkFacade yandexGamesSdkFacade = new YandexGamesSdkFacade(_registerWindowLoader.Load());
#endif

			MainMenuFactory mainMenuFactory = new MainMenuFactory(
				_assetResolver,
				_leaderBoardService,
				_translatorService
			);

			await _sceneLoader.Load(ConstantNames.MenuScene);
			await mainMenuFactory.Create();

			MainMenuBehaviour mainMenuBehaviour = mainMenuFactory.MainMenuBehaviour;

			_mainMenuPresenter = new MainMenuPresenter(
				mainMenuBehaviour,
				_levelProgressFacade,
				_gameStateMachine,
				_levelConfigGetter,
				 _progressLoadDataService
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