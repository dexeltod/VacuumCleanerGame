using System;
using Sources.ServicesInterfaces.StateMachine;

namespace Sources.Infrastructure.Factories.Player
{
	public class StartState : IState
	{
		public event Action<IState> StateChanged;
		public void Enter()
		{
		}

		public void Exit()
		{
		}
	}
}