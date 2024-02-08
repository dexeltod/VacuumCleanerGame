namespace Sources.InfrastructureInterfaces.States.StateMachineInterfaces
{
	public interface IGameState : IExitState
	{
		void Enter();
	}

	public interface IGameState<in T> : IExitState
	{
		void Enter(T payload);
	}
}