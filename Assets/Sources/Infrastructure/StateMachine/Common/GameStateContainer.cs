using System;
using Sources.InfrastructureInterfaces.States.StateMachineInterfaces;

namespace Sources.Infrastructure.StateMachine.Common
{
	public class GameStateContainer : IGameStateContainer
	{
		public IExitState ActiveState { get; private set; }

		public void Set<TState>(TState payload) where TState : class, IExitState =>
			ActiveState = payload ?? throw new ArgumentNullException(nameof(payload));
	}
}