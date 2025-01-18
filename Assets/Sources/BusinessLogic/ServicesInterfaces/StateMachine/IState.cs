using System;

namespace Sources.BusinessLogic.ServicesInterfaces.StateMachine
{
	public interface IState
	{
		public event Action<IState> StateChanged;

		void Enter();
		void Exit();
	}
}
