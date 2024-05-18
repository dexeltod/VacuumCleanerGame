using Sources.InfrastructureInterfaces.States.StateMachineInterfaces;

namespace Sources.InfrastructureInterfaces.Repository
{
	public interface IGameStateMachineRepository
	{
		IExitableState Get<TState>() where TState : IExitableState;
		void Set<TState>(TState state) where TState : IExitableState;
	}
}