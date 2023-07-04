using Infrastructure.StateMachine.GameStates;

namespace Infrastructure.StateMachine
{
	public interface IGameState : IExitState
	{
		void Enter();
	}
}