using System;

namespace Model
{
	public interface IState
	{
		public event Action<IState> StateChanged;
		
		void Enter();
		void Exit();
	}
}