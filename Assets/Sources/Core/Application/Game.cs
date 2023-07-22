using Sources.Core.Application.StateMachine;
using Sources.Core.DI;

namespace Sources.Core.Application
{
	public class Game
	{
		public readonly GameStateMachine StateMachine;

		public Game(ICoroutineRunner coroutineRunner, LoadingCurtain loadingCurtain)
		{
			StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), loadingCurtain, ServiceLocator.Container);
		}
	}
}