using Cysharp.Threading.Tasks;
using Sources.Application.StateMachineInterfaces;
using Sources.Utils.Configs;
using Sources.View.SceneEntity;

namespace Sources.Application.StateMachine.GameStates
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

		public async UniTask Enter()
		{
			_sceneLoader.Load(ConstantNames.MenuScene);
		}
		public void Exit()
		{
			_loadingCurtain.gameObject.SetActive(true);
		}
	}
}