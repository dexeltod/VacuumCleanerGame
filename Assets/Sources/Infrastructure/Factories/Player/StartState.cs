using System;
using Sources.Core.Domain.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.Core.Application.StateMachine
{
	public class StartState : IState
	{
		public event Action<IState> StateChanged;
		public void Enter()
		{
		}

		public void Exit()
		{
		}
	}
}