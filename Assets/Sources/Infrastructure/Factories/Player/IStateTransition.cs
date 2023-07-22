using System;
using Sources.Core.Domain.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.Core.Application.StateMachine.GameStates
{
	public interface IStateTransition
	{
		event Action<IState> StateChanged;
		void OnEnable();
		void OnDisable();
	}
}