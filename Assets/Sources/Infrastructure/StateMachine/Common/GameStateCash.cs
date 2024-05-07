using System;
using Sources.InfrastructureInterfaces.States.StateMachineInterfaces;

namespace Sources.Infrastructure.StateMachine.Common
{
	public sealed class GameStateCash : IGameStateCash
	{
		public IExitState ActiveState { get; private set; }

		public void Set<TState>(TState payload) where TState : class, IExitState =>
			ActiveState = payload ?? throw new ArgumentNullException(nameof(payload));
	}
}