using Sources.DIService;
using Sources.ServicesInterfaces;

namespace Sources.Application.StateMachineInterfaces
{
	public interface IGameStateMachine : IService
	{
		void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>;

		void Enter<TState, TPayload, T>(TPayload payload, string music, bool isLevelNameIsStopMusicBetweenScenes)
			where TState : class, IPayloadState<TPayload>;
	}
}