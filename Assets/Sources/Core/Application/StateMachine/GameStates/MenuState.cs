using Sources.Core.Application.StateMachineInterfaces;
using Sources.Core.Utils.Configs;

namespace Sources.Core.Application.StateMachine.GameStates
{
	public class MenuState : IGameState
	{
		private readonly SceneLoader _sceneLoader;
		private readonly LoadingCurtain _loadingCurtain;

		public MenuState(SceneLoader sceneLoader, LoadingCurtain loadingCurtain)
		{
			_sceneLoader = sceneLoader;
			_loadingCurtain = loadingCurtain;
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