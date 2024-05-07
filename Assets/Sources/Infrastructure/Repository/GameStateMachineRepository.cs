using System;
using System.Collections.Generic;
using Sources.InfrastructureInterfaces.Repository;
using Sources.InfrastructureInterfaces.States.StateMachineInterfaces;
using UnityEngine;

namespace Sources.Infrastructure.Repository
{
	public sealed class GameStateMachineRepository : IGameStateMachineRepository
	{
		private readonly Dictionary<Type, IExitState> _states = new();

		public IExitState Get<TState>() where TState : class, IExitState
		{
			if (_states.ContainsKey(typeof(TState)))
				return _states[typeof(TState)];

			throw new ArgumentNullException($"State of type {typeof(TState)} not found");
		}

		public void Set<TState>(TState state) where TState : class, IExitState
		{
			if (state == null)
				throw new ArgumentNullException(nameof(state));

			Debug.Log("Добавляем хуйню");
			_states.Add(typeof(TState), state);
		}
	}
}