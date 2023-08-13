using Sources.Application.StateMachine.GameStates;

namespace Sources.Application.StateMachineInterfaces
{
	public interface IPayloadState<TPayload> : IExitState
	{
		void Enter(TPayload payload);
	}
}