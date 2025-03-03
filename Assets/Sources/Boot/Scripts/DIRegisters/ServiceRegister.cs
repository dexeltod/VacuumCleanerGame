using System;
using Sources.Boot.Scripts.Factories.Presentation.Scene;
using Sources.Boot.Scripts.Factories.StateMachine;
using Sources.Boot.Scripts.States.StateMachine.Common;
using Sources.BusinessLogic;
using Sources.BusinessLogic.Interfaces;
using Sources.BusinessLogic.Repository;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.BusinessLogic.States.StateMachineInterfaces;
using Sources.Controllers.Services;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Leaderboard;
using Sources.Infrastructure.Repository;
using Sources.Infrastructure.Services;
using Sources.Infrastructure.Services.DomainServices;
using Sources.PresentationInterfaces.Common;
using UnityEngine;
using VContainer;
using VContainer.Unity;
#if YANDEX_CODE
#endif

namespace Sources.Boot.Scripts.DIRegisters
{
	public class ServiceRegister
	{
		private readonly IContainerBuilder _builder;

		public ServiceRegister(IContainerBuilder builder) =>
			_builder = builder ?? throw new ArgumentNullException(nameof(builder));

		public void Register()
		{
			_builder.RegisterEntryPoint<GameBuilder>();

			_builder
				.UseAssetLoader()
				.UseInjectableAssetLoader()
				.UseProgress()
				.UseFactories()
				.UseSaveLoader();

			#region BaseServices

			_builder.Register<ISceneLoader, SceneLoader>(Lifetime.Singleton);
			_builder.Register<ILocalizationService, LocalizationService>(Lifetime.Singleton);

			_builder.Register<GameStateContainer>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

			_builder
				.Register(
					resolver =>
					{
						var container = resolver.Resolve<IGameStateContainer>();
						var repository = resolver.Resolve<IGameStateMachineRepository>();
						return new StateMachine(container, repository);
					},
					Lifetime.Singleton
				)
				.AsImplementedInterfaces()
				.AsSelf();

			#region States

			new StatesRegister(_builder).Register();

			#endregion

			_builder
				.Register(
					container => new PlayerModelRepository(
						container.Resolve<IPersistentProgressService>().GlobalProgress.PlayerStatsModel
					),
					Lifetime.Singleton
				)
				.AsImplementedInterfaces()
				.AsSelf();

			RegisterLoadingCurtain();

			#endregion

			_builder
				.UseCoroutineRunner()
				.UseGameFocusHandler()
				.UseTranslator()
				.UseFactories();

			new ProviderRegister(_builder).Register();

			#region InitializeProgressServices

			_builder.Register<ILevelConfigGetter, LevelConfigGetter>(Lifetime.Singleton);
			_builder.Register<ILevelProgressFacade, LevelProgressFacade>(Lifetime.Singleton);

			#endregion

			#region Repositories

			_builder.Register<GameStateMachineRepository>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

			_builder.UseProgressEntityRepository();

			_builder.Register<IActiveRepository<IPresenter>, PresentersRepository>(Lifetime.Singleton).AsSelf();
			_builder.Register<IRepository<IView>, ViewsRepository>(Lifetime.Singleton).AsSelf();

			_builder.Register<GameStatesRepositoryInitializer>(Lifetime.Scoped).AsSelf();

			_builder
				.Register(_ => new LeaderBoardRepository(GetLeaderboard()), Lifetime.Singleton)
				.AsSelf()
				.AsImplementedInterfaces();

			#endregion

			_builder.RegisterEntryPointExceptionHandler(
				exception => Debug.LogError(exception.Message)
			);
		}

		private IAbstractLeaderBoard GetLeaderboard()
		{
#if YANDEX_CODE
			return new YandexLeaderboard();
#endif
			return new TestLeaderBoardService();
		}

		private void RegisterLoadingCurtain() =>
			_builder
				.Register(
					container => new LoadingCurtainFactory(container.Resolve<IAssetLoader>()).Create(),
					Lifetime.Singleton
				)
				.AsImplementedInterfaces()
				.AsSelf();
	}
}