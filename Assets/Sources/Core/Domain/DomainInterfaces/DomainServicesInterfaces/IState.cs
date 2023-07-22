using System;

namespace Sources.Core.Domain.DomainInterfaces.DomainServicesInterfaces
{
	public interface IState
	{
		public event Action<IState> StateChanged;
		
		void Enter();
		void Exit();
	}
}