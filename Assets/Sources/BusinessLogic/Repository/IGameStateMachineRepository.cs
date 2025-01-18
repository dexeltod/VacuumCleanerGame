using Sources.BuisenessLogic.States.StateMachineInterfaces;

namespace Sources.BuisenessLogic.Repository
{
	public interface IGameStateMachineRepository
	{
		IExitableState Get<TState>() where TState : IExitableState;
		void Set<TState>(TState state) where TState : class, IExitableState;
	}
}