using Sources.Application.StateMachine.GameStates;

namespace Sources.Application.StateMachineInterfaces
{
	public interface IGameState : IExitState
	{
		void Enter();
	}
}