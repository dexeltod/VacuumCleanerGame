using Model.Infrastructure.StateMachine.GameStates;

namespace Model.Infrastructure.StateMachine
{
	public interface IPayloadState<TPayload> : IExitState
	{
		void Enter(TPayload payload);
	}
}