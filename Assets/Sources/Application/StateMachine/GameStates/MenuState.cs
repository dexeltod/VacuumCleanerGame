using System;
using Sources.ApplicationServicesInterfaces;
using Sources.ApplicationServicesInterfaces.StateMachineInterfaces;
using Sources.DIService;
using Sources.Infrastructure.Factories.LeaderBoard;
using Sources.Infrastructure.Presenters;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Scene;
using Sources.Presentation.SceneEntity;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs.Scripts;

namespace Sources.Application.StateMachine.GameStates
{
	public sealed class MenuState : IGameState
	{
		private readonly ISceneLoader _sceneLoader;
		private readonly LoadingCurtain _loadingCurtain;
		private readonly ServiceLocator _serviceLocator;
		private MainMenuPresenter _mainMenuPresenter;

		public MenuState(ISceneLoader sceneLoader, LoadingCurtain loadingCurtain, ServiceLocator serviceLocator)
		{
			_sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
			_loadingCurtain = loadingCurtain ? loadingCurtain : throw new ArgumentNullException(nameof(loadingCurtain));
			_serviceLocator = serviceLocator ?? throw new ArgumentNullException(nameof(serviceLocator));
		}

		public async void Enter()
		{
			#region GettingServices

			IAssetProvider assetProvider = _serviceLocator.Get<IAssetProvider>();
			ILevelProgressFacade levelProgress = _serviceLocator.Get<ILevelProgressFacade>();
			IGameStateMachine gameStateMachine = _serviceLocator.Get<IGameStateMachine>();
			ILevelConfigGetter levelConfigGetter = _serviceLocator.Get<ILevelConfigGetter>();

#if !UNITY_EDITOR
			_serviceLocator.Get<IYandexSDKController>().SetStatusInitialized();
#endif
			ILeaderBoardService leaderBoardService = _serviceLocator.Get<ILeaderBoardService>();

			#endregion

			MainMenuFactory mainMenuFactory = new MainMenuFactory(assetProvider, leaderBoardService);

			await _sceneLoader.Load(ConstantNames.MenuScene);
			await mainMenuFactory.Instantiate();

			MainMenuBehaviour mainMenuBehaviour = mainMenuFactory.MainMenuBehaviour;

			_mainMenuPresenter = new MainMenuPresenter(
				mainMenuBehaviour,
				levelProgress,
				gameStateMachine,
				levelConfigGetter
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