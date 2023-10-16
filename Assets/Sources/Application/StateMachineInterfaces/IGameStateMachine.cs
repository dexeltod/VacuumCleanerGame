using Sources.DIService;

namespace Sources.Application.StateMachineInterfaces
{
	public interface IGameStateMachine : IService
	{
		void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>;
	}
}