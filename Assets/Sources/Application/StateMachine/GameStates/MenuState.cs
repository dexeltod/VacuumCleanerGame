using Sources.Application.StateMachineInterfaces;
using Sources.ApplicationServicesInterfaces;
using Sources.DIService;
using Sources.Infrastructure.Factories.LeaderBoard;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs;
using Sources.View.SceneEntity;

namespace Sources.Application.StateMachine.GameStates
{
	public sealed class MenuState : IGameState
	{
		private readonly SceneLoader _sceneLoader;
		private readonly LoadingCurtain _loadingCurtain;
		private readonly ServiceLocator _serviceLocator;

		public MenuState(SceneLoader sceneLoader, LoadingCurtain loadingCurtain, ServiceLocator serviceLocator)
		{
			_sceneLoader = sceneLoader;
			_loadingCurtain = loadingCurtain;
			_serviceLocator = serviceLocator;
		}

		public async void Enter()
		{
			#region GettingServices

			IAssetProvider assetProvider = _serviceLocator.Get<IAssetProvider>();
			ILeaderBoardService leaderBoardService = _serviceLocator.Get<ILeaderBoardService>();

			#endregion

			LeaderBoardFactory leaderBoardFactory = new LeaderBoardFactory(assetProvider, leaderBoardService);

			await _sceneLoader.Load(ConstantNames.MenuScene);
#if !UNITY_EDITOR
			await leaderBoardFactory.Create();
#endif

			_loadingCurtain.HideSlowly();
		}

		public void Exit()
		{
			_loadingCurtain.Show();
			_loadingCurtain.gameObject.SetActive(true);
		}
	}
}