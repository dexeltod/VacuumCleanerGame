using System;

namespace Infrastructure.StateMachine
{
	public interface IState
	{
		public event Action<IState> StateChanged;
		
		void Enter();
		void Exit();
	}
}