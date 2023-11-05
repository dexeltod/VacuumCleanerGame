using System;
using System.Collections.Generic;
using Sources.Application.StateMachine.GameStates;
using Sources.Application.StateMachineInterfaces;

namespace Sources.Application.StateMachine
{
	public class GameStateMachine : IGameStateMachine
	{
		private IExitState _activeState;

		private Dictionary<Type, IExitState> _states;

		public void Initialize(Dictionary<Type, IExitState> states) =>
			_states = states;

		public void Enter<TState>() where TState : class, IGameState
		{
			TState state = ChangeState<TState>();
			state.Enter();
		}

		public void Enter<TState, TPayload>(TPayload payload)
			where TState : class, IPayloadState<TPayload>
		{
			TState state = ChangeState<TState>();
			state.Enter(payload);
		}

		private TState ChangeState<TState>() where TState : class, IExitState
		{
			
			_activeState?.Exit();

			TState state = GetState<TState>();
			_activeState = state;

			return state;
		}

		private TState GetState<TState>() where TState : class, IExitState =>
			_states[typeof(TState)] as TState;
	}
}