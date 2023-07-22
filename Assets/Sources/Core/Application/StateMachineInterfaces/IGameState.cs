using Sources.Core.Application.StateMachine.GameStates;

namespace Sources.Core.Application.StateMachineInterfaces
{
	public interface IGameState : IExitState
	{
		void Enter();
	}
}