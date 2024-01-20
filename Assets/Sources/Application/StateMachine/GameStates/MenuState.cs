using System;
using System.Runtime.InteropServices;
using Sources.ApplicationServicesInterfaces;
using Sources.ApplicationServicesInterfaces.StateMachineInterfaces;
using Sources.Infrastructure.Factories.LeaderBoard;
using Sources.Infrastructure.Presenters;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Scene;
using Sources.Presentation.SceneEntity;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs.Scripts;
using VContainer;

namespace Sources.Application.StateMachine.GameStates
{
	public sealed class MenuState : IGameState
	{
		private readonly ISceneLoader _sceneLoader;
		private readonly LoadingCurtain _loadingCurtain;
		private readonly IAssetProvider _assetProvider;
		private readonly ILevelProgressFacade _levelProgressFacade;
		private readonly IGameStateMachine _gameStateMachine;
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly ILeaderBoardService _leaderBoardService;

		private MainMenuPresenter _mainMenuPresenter;

		[Inject]
		public MenuState(
			ISceneLoader sceneLoader,
			LoadingCurtain loadingCurtain,
			IAssetProvider assetProvider,
			ILevelProgressFacade levelProgressFacade,
			IGameStateMachine gameStateMachine,
			ILevelConfigGetter levelConfigGetter,
			ILeaderBoardService leaderBoardService
		)
		{
			_levelConfigGetter = levelConfigGetter ?? throw new ArgumentNullException(nameof(levelConfigGetter));
			_leaderBoardService = leaderBoardService ?? throw new ArgumentNullException(nameof(leaderBoardService));
			_gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
			_levelProgressFacade = levelProgressFacade ?? throw new ArgumentNullException(nameof(levelProgressFacade));
			_assetProvider = assetProvider ?? throw new ArgumentNullException(nameof(assetProvider));
			_sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
			_loadingCurtain = loadingCurtain ? loadingCurtain : throw new ArgumentNullException(nameof(loadingCurtain));
		}

		public async void Enter()
		{
			MainMenuFactory mainMenuFactory = new MainMenuFactory(_assetProvider, _leaderBoardService);

			await _sceneLoader.Load(ConstantNames.MenuScene);
			await mainMenuFactory.Instantiate();

			MainMenuBehaviour mainMenuBehaviour = mainMenuFactory.MainMenuBehaviour;

			_mainMenuPresenter = new MainMenuPresenter(
				mainMenuBehaviour,
				_levelProgressFacade,
				_gameStateMachine,
				_levelConfigGetter
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