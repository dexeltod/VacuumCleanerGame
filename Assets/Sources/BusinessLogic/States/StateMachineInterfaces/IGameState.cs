namespace Sources.BuisenessLogic.States.StateMachineInterfaces
{
	public interface IGameState : IExitableState
	{
		void Enter();
	}

	public interface IGameState<in T> : IExitableState
	{
		void Enter(T payload);
	}
}