using ViewModel.Infrastructure.StateMachine.GameStates;

namespace ViewModel.Infrastructure.StateMachine
{
	public interface IPayloadState<TPayload> : IExitState
	{
		void Enter(TPayload payload);
	}
}