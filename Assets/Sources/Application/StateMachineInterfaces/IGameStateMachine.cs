using Cysharp.Threading.Tasks;
using Sources.DIService;

namespace Sources.Application.StateMachineInterfaces
{
	public interface IGameStateMachine : IService
	{
		UniTask Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>;

		UniTask Enter<TState, TPayload, T>(TPayload payload, string music, bool isLevelNameIsStopMusicBetweenScenes)
			where TState : class, IPayloadState<TPayload>;
	}
}