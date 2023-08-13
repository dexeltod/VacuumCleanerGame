using System;
using Sources.ServicesInterfaces.StateMachine;

namespace Sources.Infrastructure.Factories.Player
{
	public interface IStateTransition
	{
		event Action<IState> StateChanged;
		void OnEnable();
		void OnDisable();
	}
}