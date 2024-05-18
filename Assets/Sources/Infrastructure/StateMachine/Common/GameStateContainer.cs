using System;
using Sources.InfrastructureInterfaces.States.StateMachineInterfaces;

namespace Sources.Infrastructure.StateMachine.Common
{
	public class GameStateContainer : IGameStateContainer
	{
		public IExitableState ActiveState { get; private set; }

		public void Set<TState>(TState payload) where TState : class, IExitableState =>
			ActiveState = payload ?? throw new ArgumentNullException(nameof(payload));
	}
}