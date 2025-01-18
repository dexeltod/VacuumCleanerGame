using Sources.BusinessLogic.States.StateMachineInterfaces;

namespace Sources.BusinessLogic.Repository
{
	public interface IGameStateMachineRepository
	{
		IExitableState Get<TState>() where TState : IExitableState;
		void Set<TState>(TState state) where TState : class, IExitableState;
	}
}
