using Model;
using Model.DI;
using View.UI;
using ViewModel.Infrastructure.Services;
using ViewModel.Infrastructure.StateMachine;
using ViewModel.Infrastructure.StateMachine.GameStates;

namespace ViewModel.Infrastructure
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