namespace Sources.InfrastructureInterfaces.States.StateMachineInterfaces
{
	public interface IGameStateContainer
	{
		IExitableState ActiveState { get; }

		public void Set<TState>(TState payload) where TState : class, IExitableState;
	}
}