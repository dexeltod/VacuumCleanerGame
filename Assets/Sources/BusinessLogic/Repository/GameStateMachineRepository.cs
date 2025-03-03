using System;
using System.Collections.Generic;
using Sources.BusinessLogic.States.StateMachineInterfaces;

namespace Sources.BusinessLogic.Repository
{
	public sealed class GameStateMachineRepository : IGameStateMachineRepository
	{
		private readonly Dictionary<Type, IExitableState> _states = new();

		public IExitableState Get<TState>() where TState : IExitableState
		{
			Type huy = typeof(TState);

			if (_states.ContainsKey(typeof(TState)))
				return _states[huy];

			throw new ArgumentNullException($"State of type {typeof(TState)} not found");
		}

		public void Set<TState>(TState state) where TState : class, IExitableState
		{
			if (state == null)
				throw new ArgumentNullException(nameof(state));

			Type type = typeof(TState);
			_states.Add(type, state);
		}
	}
}