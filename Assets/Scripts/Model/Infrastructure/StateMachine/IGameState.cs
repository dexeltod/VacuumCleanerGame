using Model.Infrastructure.StateMachine.GameStates;

namespace Model.Infrastructure.StateMachine
{
	public interface IGameState : IExitState
	{
		void Enter();
	}
}