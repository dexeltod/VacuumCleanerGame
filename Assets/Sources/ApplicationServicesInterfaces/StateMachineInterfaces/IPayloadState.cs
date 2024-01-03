using Cysharp.Threading.Tasks;

namespace Sources.ApplicationServicesInterfaces.StateMachineInterfaces
{
	public interface IPayloadState<in TPayload> : IExitState
	{
		UniTask Enter(TPayload payload);
	}
}