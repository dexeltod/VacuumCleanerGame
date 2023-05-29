using ViewModel.Infrastructure.StateMachine.GameStates;

namespace ViewModel.Infrastructure.StateMachine
{
	public interface IGameState : IExitState
	{
		void Enter();
	}
}