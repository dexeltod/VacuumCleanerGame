using System;
using System.Collections.Generic;
using Sources.BuisenessLogic.States.StateMachineInterfaces;

namespace Sources.BuisenessLogic.Repository
{
	public sealed class GameStateMachineRepository : IGameStateMachineRepository
	{
		private readonly Dictionary<Type, IExitableState> _states = new();

		public IExitableState Get<TState>() where TState : IExitableState
		{
			if (_states.ContainsKey(typeof(TState)))
				return _states[typeof(TState)];

			throw new ArgumentNullException($"State of type {typeof(TState)} not found");
		}

		public void Set<TState>(TState state) where TState : class, IExitableState
		{
			if (state == null)
				throw new ArgumentNullException(nameof(state));

			_states.Add(typeof(TState), state);
		}
	}
}