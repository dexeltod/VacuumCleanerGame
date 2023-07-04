using Infrastructure.StateMachine.GameStates;

namespace Infrastructure.StateMachine
{
	public interface IPayloadState<TPayload> : IExitState
	{
		void Enter(TPayload payload);
	}
}