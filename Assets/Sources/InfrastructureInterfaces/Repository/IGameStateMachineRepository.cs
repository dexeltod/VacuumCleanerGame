using Sources.InfrastructureInterfaces.States.StateMachineInterfaces;

namespace Sources.InfrastructureInterfaces.Repository
{
	public interface IGameStateMachineRepository
	{
		IExitState Get<TState>() where TState : class, IExitState;
		void Set<TState>(TState state) where TState : class, IExitState;
	}
}