using System;

namespace ViewModel.Infrastructure.StateMachine
{
	public interface IState
	{
		public event Action<IState> StateChanged;
		
		void Enter();
		void Exit();
	}
}