using System;
using Sources.Boot.Scripts.Factories.Presentation.LeaderBoard;
using VContainer;

namespace Sources.Boot.Scripts.DIRegisters
{
	public class FactoriesRegister
	{
		private readonly IContainerBuilder _builder;

		public FactoriesRegister(IContainerBuilder builder) =>
			_builder = builder ?? throw new ArgumentNullException(nameof(builder));

		public void Register()
		{
			_builder.Register<LeaderBoardPlayersFactory>(Lifetime.Scoped).AsSelf().AsImplementedInterfaces();
		}
	}
}
