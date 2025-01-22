using System;

namespace Sources.BusinessLogic.ServicesInterfaces.StateMachine
{
	public interface IState
	{
		void Enter();
		void Exit();
		public event Action<IState> StateChanged;
	}
}