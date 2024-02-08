using Sources.InfrastructureInterfaces.States.StateMachineInterfaces;

namespace Sources.InfrastructureInterfaces.Services
{
	public interface IGameStateChanger
	{
		void Enter<TState>() where TState : class, IGameState;
		void Enter<TState, TPayload>(TPayload payload) where TState : class, IGameState<TPayload>;
	}
}