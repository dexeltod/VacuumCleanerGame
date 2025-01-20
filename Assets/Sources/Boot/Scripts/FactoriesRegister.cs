using System;
using Sources.Boot.Scripts.Factories;
using Sources.Boot.Scripts.Factories.Domain;
using Sources.Boot.Scripts.Factories.Presentation.LeaderBoard;
using Sources.Boot.Scripts.Factories.Presentation.Scene;
using Sources.Boot.Scripts.Factories.Progress;
using Sources.BusinessLogic.Interfaces.Factory;
using Sources.Controllers.Factories;
using VContainer;

namespace Sources.Boot.Scripts
{
	public class FactoriesRegister
	{
		private readonly IContainerBuilder _builder;

		public FactoriesRegister(IContainerBuilder builder)
		{
			_builder = builder ?? throw new ArgumentNullException(nameof(builder));
		}

		public void Register()
		{
			_builder.Register<ShopModelFactory>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
			_builder.Register<ResourcePathConfigServiceFactory>(Lifetime.Scoped);

			_builder.Register<LeaderBoardPlayersFactory>(Lifetime.Scoped).AsSelf().AsImplementedInterfaces();

			_builder.Register<SaveLoaderFactory>(
				Lifetime.Scoped
			).AsImplementedInterfaces().AsSelf();

			_builder.Register<InitialProgressFactory>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
			_builder.Register<CoroutineRunnerFactory>(Lifetime.Scoped);
			_builder.Register<LoadingCurtainFactory>(Lifetime.Scoped);

			_builder.Register<ProgressFactory>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
			_builder.Register<ResourcePathConfigServiceFactory>(Lifetime.Singleton);

			_builder.Register<IPlayerFactory, PlayerFactory>(Lifetime.Singleton);
		}
	}
}