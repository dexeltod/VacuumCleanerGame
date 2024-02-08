namespace Sources.InfrastructureInterfaces.States.StateMachineInterfaces
{
	public interface IGameStateContainer
	{
		IExitState ActiveState { get; }

		public void Set<TState>(TState payload) where TState : class, IExitState;
	}
}