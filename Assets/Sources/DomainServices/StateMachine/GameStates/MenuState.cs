using Application.Configs;
using Application.DI;
using InfrastructureInterfaces;
using View.UI;

namespace Infrastructure.StateMachine.GameStates
{
	public class MenuState : IGameState
	{
		private readonly SceneLoader _sceneLoader;
		private readonly LoadingCurtain _loadingCurtain;
		private readonly IMusicService _musicService;

		public MenuState(SceneLoader sceneLoader, LoadingCurtain loadingCurtain)
		{
			_sceneLoader = sceneLoader;
			_loadingCurtain = loadingCurtain;
			_musicService = ServiceLocator.Container.GetSingle<IMusicService>();
		}

		public void Enter()
		{
			_sceneLoader.Load(ConstantNames.MenuScene);
		}
		public void Exit()
		{
			_loadingCurtain.gameObject.SetActive(true);
		}
	}
}