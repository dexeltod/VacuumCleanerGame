using System;
using Sources.InfrastructureInterfaces.Repository;
using Sources.InfrastructureInterfaces.Services;
using Sources.InfrastructureInterfaces.States.StateMachineInterfaces;

namespace Sources.Infrastructure.Services
{
	public sealed class GameStateChanger : IGameStateChanger
	{
		private readonly IGameStateCash _gameStateCash;
		private readonly IGameStateMachineRepository _gameStateMachineRepository;

		public GameStateChanger(
			IGameStateCash gameStateCash,
			IGameStateMachineRepository gameStateMachineRepository
		)
		{
			_gameStateCash = gameStateCash ?? throw new ArgumentNullException(nameof(gameStateCash));
			_gameStateMachineRepository = gameStateMachineRepository ??
				throw new ArgumentNullException(nameof(gameStateMachineRepository));
		}

		public void Enter<TState>() where TState : class, IGameState
		{
			TState state = ChangeState<TState>();

			if (state == null)
				throw new InvalidOperationException("State of type " + typeof(TState) + " not found");

			state.Enter();
		}

		public void Enter<TState, TPayload>(TPayload payload) where TState : class, IGameState<TPayload>
		{
			IGameState<TPayload> state = ChangeState<TState>();

			state.Enter(payload);
		}

		private TState ChangeState<TState>() where TState : class, IExitState
		{
			_gameStateCash.ActiveState?.Exit();

			IExitState state = _gameStateMachineRepository.Get<TState>();
			_gameStateCash.Set(state);

			return (TState)state;
		}
	}
}