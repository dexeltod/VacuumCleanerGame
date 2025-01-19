using System;
using System.Collections.Generic;
using Sources.BusinessLogic.ServicesInterfaces.StateMachine;

namespace Sources.Boot.Scripts.Factories.Player
{
	public class StateService
	{
		private readonly Dictionary<Type, IState> _states = new();

		public void Register<T>(T instance) where T : class, IState
		{
			if (_states.ContainsKey(typeof(T)))
				throw new InvalidOperationException();

			_states.Add(typeof(T), instance);
		}

		public T Get<T>() where T : class, IState
		{
			if (_states.ContainsKey(typeof(T)) == false)
				throw new InvalidOperationException();

			return _states[typeof(T)] as T;
		}
	}
}