using System;
using Sources.BusinessLogic.Repository;
using Sources.BusinessLogic.Services;
using Sources.BusinessLogic.States.StateMachineInterfaces;

namespace Sources.BusinessLogic
{
	public sealed class GameStateChanger : IGameStateChanger
	{
		private readonly IGameStateContainer _gameStateContainer;
		private readonly IGameStateMachineRepository _gameStateMachineRepository;

		public GameStateChanger(
			IGameStateContainer gameStateContainer,
			IGameStateMachineRepository gameStateMachineRepository
		)
		{
			_gameStateContainer = gameStateContainer ?? throw new ArgumentNullException(nameof(gameStateContainer));

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

		private TState ChangeState<TState>() where TState : class, IExitableState
		{
			_gameStateContainer.ActiveState?.Exit();

			IExitableState state = _gameStateMachineRepository.Get<TState>();
			_gameStateContainer.Set(state);

			return (TState)state;
		}
	}
}
