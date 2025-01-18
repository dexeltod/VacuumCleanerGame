using Sources.BuisenessLogic.States.StateMachineInterfaces;

namespace Sources.BuisenessLogic.Services
{
	public interface IGameStateChanger
	{
		void Enter<TState>() where TState : class, IGameState;

		void Enter<TState, TPayload>(TPayload payload) where TState : class, IGameState<TPayload>;
	}
}