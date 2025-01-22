using System;
using Sources.BusinessLogic.Repository;
using Sources.BusinessLogic.Services;
using Sources.BusinessLogic.States.StateMachineInterfaces;
using VContainer;

namespace Sources.BusinessLogic
{
	public sealed class StateMachine : IStateMachine
	{
		private readonly IGameStateContainer _container;
		private readonly IGameStateMachineRepository _repository;

		[Inject]
		public StateMachine(
			IGameStateContainer gameStateContainer,
			IGameStateMachineRepository gameStateMachineRepository
		)
		{
			_container = gameStateContainer ?? throw new ArgumentNullException(nameof(gameStateContainer));
			_repository = gameStateMachineRepository ?? throw new ArgumentNullException(nameof(gameStateMachineRepository));
		}

		public void Enter<TState>() where TState : class, IGameState
		{
			var state = ChangeState<TState>();

			if (state == null)
				throw new InvalidOperationException("State of type " + typeof(TState) + " not found");

			state.Enter();
			_container.Set(state);
		}

		public void Enter<TState, TPayload>(TPayload payload) where TState : class, IGameState<TPayload> =>
			ChangeState<TState>().Enter(payload);

		private TState ChangeState<TState>() where TState : class, IExitableState
		{
			_container.ActiveState?.Exit();

			return (TState)_repository.Get<TState>();
		}
	}
}