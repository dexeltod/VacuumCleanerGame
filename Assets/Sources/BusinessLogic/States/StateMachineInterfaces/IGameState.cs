using Cysharp.Threading.Tasks;

namespace Sources.BusinessLogic.States.StateMachineInterfaces
{
	public interface IGameState : IExitableState
	{
		UniTask Enter();
	}

	public interface IGameState<in T> : IExitableState
	{
		UniTask Enter(T payload);
	}
}
