using Sources.Application.StateMachine;
using Sources.Application.StateMachine.GameStates;

namespace Sources.Application
{
	public class Game
	{
		private readonly GameStateMachine _gameStateMachine;

		public Game(GameStateMachine gameStateMachine) =>
			_gameStateMachine = gameStateMachine;

		public void Start() =>
			_gameStateMachine.Enter<InitializeServicesAndProgressState>();
	}
}