using System;
using Sources.Boot.Scripts.States.StateMachine.GameStates;
using VContainer;

namespace Sources.Boot.Scripts.DIRegisters
{
	public class StatesRegister
	{
		private readonly IContainerBuilder _builder;

		public StatesRegister(IContainerBuilder builder) =>
			_builder = builder ?? throw new ArgumentNullException(nameof(builder));

		public void Register()
		{
			_builder.Register<MenuState>(Lifetime.Scoped).AsImplementedInterfaces();
			_builder.Register<BuildSceneState>(Lifetime.Scoped).AsImplementedInterfaces();

			_builder.Register<GameLoopState>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
		}
	}
}