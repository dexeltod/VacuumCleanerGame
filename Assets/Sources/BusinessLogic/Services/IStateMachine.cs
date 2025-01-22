using Sources.BusinessLogic.States.StateMachineInterfaces;

namespace Sources.BusinessLogic.Services
{
	public interface IStateMachine
	{
		void Enter<TState>() where TState : class, IGameState;

		void Enter<TState, TPayload>(TPayload payload) where TState : class, IGameState<TPayload>;
	}
}