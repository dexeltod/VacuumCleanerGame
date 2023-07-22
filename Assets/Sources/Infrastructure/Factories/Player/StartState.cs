using System;
using Sources.Core.Domain.DomainInterfaces.DomainServicesInterfaces;
using UnityEngine;

namespace Sources.Core.Application.StateMachine
{
	public class StartState : IState
	{
		public event Action<IState> StateChanged;
		public void Enter()
		{
			Debug.Log("need implementation");
		}

		public void Exit()
		{
			Debug.Log("need implementation");
		}
	}
}