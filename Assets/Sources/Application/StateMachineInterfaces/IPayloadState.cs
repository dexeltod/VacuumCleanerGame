using Cysharp.Threading.Tasks;
using Sources.Application.StateMachine.GameStates;

namespace Sources.Application.StateMachineInterfaces
{
	public interface IPayloadState<TPayload> : IExitState
	{
		UniTask Enter(TPayload payload);
	}
}