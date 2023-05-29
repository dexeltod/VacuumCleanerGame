using System;

namespace ViewModel.Infrastructure.StateMachine.GameStates
{
	public interface IStateTransition
	{
		event Action<IState> StateChanged;
		void OnEnable();
		void OnDisable();
	}
}