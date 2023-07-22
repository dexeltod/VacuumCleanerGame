using Sources.Core.Application.StateMachineInterfaces;

namespace Sources.Core.Application.StateMachine.GameStates
{
	public class GameLoopState : IGameState
	{
		private readonly GameStateMachine _gameStateMachine;

		public GameLoopState(GameStateMachine gameStateMachine)
		{
			_gameStateMachine = gameStateMachine;
		}
		
		public void Exit()
		{
			
		}

		public void Enter()
		{
		}
	}
}