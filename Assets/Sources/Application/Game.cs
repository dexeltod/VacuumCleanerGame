using Sources.Application.StateMachine;
using Sources.DIService;
using Sources.View.SceneEntity;

namespace Sources.Application
{
	public class Game
	{
		public readonly GameStateMachine StateMachine;

		public Game(ICoroutineRunner coroutineRunner, LoadingCurtain loadingCurtain)
		{
			StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), loadingCurtain, GameServices.Container);
		}
	}
}