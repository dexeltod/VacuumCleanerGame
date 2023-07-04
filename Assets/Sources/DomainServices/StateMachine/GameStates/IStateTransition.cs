using System;

namespace Infrastructure.StateMachine.GameStates
{
	public interface IStateTransition
	{
		event Action<IState> StateChanged;
		void OnEnable();
		void OnDisable();
	}
}