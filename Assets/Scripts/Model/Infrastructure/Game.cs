using Model.DI;
using Model.Infrastructure.Services;
using Model.Infrastructure.StateMachine;
using Model.Infrastructure.StateMachine.GameStates;

namespace Model.Infrastructure
{
	public class Game
	{
		public readonly GameStateMachine StateMachine;

		public Game(ICoroutineRunner coroutineRunner, LoadingCurtain loadingCurtain, MusicSetter musicSetter)
		{
			StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), loadingCurtain,
				ServiceLocator.Container,
				musicSetter);
		}
	}
}