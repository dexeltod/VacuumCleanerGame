using Application.DI;
using Infrastructure.Services;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.GameStates;
using InfrastructureInterfaces;
using View.UI;

namespace Application
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