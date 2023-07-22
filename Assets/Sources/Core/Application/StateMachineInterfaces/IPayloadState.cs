using Sources.Core.Application.StateMachine.GameStates;

namespace Sources.Core.Application.StateMachineInterfaces
{
	public interface IPayloadState<TPayload> : IExitState
	{
		void Enter(TPayload payload);
	}
}