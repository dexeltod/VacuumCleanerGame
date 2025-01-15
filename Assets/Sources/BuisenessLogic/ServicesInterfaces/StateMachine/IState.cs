using System;

namespace Sources.BuisenessLogic.ServicesInterfaces.StateMachine
{
	public interface IState
	{
		public event Action<IState> StateChanged;

		void Enter();
		void Exit();
	}
}