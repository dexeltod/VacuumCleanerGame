using System;
using System.Linq;
using Sources.Boot.Scripts.Factories;
using Sources.Boot.Scripts.Factories.Domain;
using Sources.Boot.Scripts.Factories.Presentation.Scene;
using Sources.Boot.Scripts.Factories.Progress;
using Sources.Boot.Scripts.Factories.StateMachine;
using Sources.Boot.Scripts.States.StateMachine.Common;
using Sources.Boot.Scripts.UpgradeEntitiesConfigs;
using Sources.BusinessLogic;
using Sources.BusinessLogic.Configs;
using Sources.BusinessLogic.Interfaces;
using Sources.BusinessLogic.Interfaces.Factory;
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
using Sources.InfrastructureInterfaces.Configs.Scripts;
using Sources.PresentationInterfaces.Common;
using Sources.Utils;
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

			#region BaseServices

			_builder.Register<IInjectableAssetLoader, InjectableAssetLoader>(Lifetime.Singleton).As<IInjectableAssetLoader>()
				.AsSelf();
			_builder.Register<InitialProgressFactory>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
			_builder.Register<IAssetLoader, AssetLoader>(Lifetime.Singleton);
			_builder.Register<ISceneLoader, SceneLoader>(Lifetime.Singleton);
			_builder.Register<ILocalizationService, LocalizationService>(Lifetime.Singleton);

			_builder.Register<ProgressionConfig>(Lifetime.Singleton);

			_builder.Register<ClearProgressFactory>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
			_builder.Register<ProgressFactory>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();

			_builder.Register<GameStateContainer>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
			_builder.Register(
				resolver =>
				{
					var container = resolver.Resolve<IGameStateContainer>();
					var repository = resolver.Resolve<IGameStateMachineRepository>();
					return new StateMachine(container, repository);
				},
				Lifetime.Singleton
			).AsImplementedInterfaces().AsSelf();

			#region States

			new StatesRegister(_builder).Register();

			#endregion

			_builder.Register(
				resolver => new PersistentProgressService(resolver.Resolve<IInitialProgressFactory>().Create()),
				Lifetime.Singleton
			).AsImplementedInterfaces().AsSelf();

			_builder.Register(
					container => new PlayerModelRepository(
						container.Resolve<IPersistentProgressService>().GlobalProgress.PlayerStatsModel
					),
					Lifetime.Singleton
				)
				.AsImplementedInterfaces()
				.AsSelf();
			RegisterLoadingCurtain();

			#endregion

			new ProviderRegister(_builder).Register();

			#region Factories

			new FactoriesRegister(_builder).Register();

			#endregion

			#region InitializeServicesAndProgress

			_builder.Register<IProgressSaveLoadDataService, ProgressSaveLoadDataService>(Lifetime.Singleton);

			RegisterCloudSavers();

			_builder.Register(_ => new SaveLoaderFactory().Create(), Lifetime.Singleton)
				.AsImplementedInterfaces().AsSelf();

			#endregion

			#region InitializeProgressServices

			_builder.Register<ILevelConfigGetter, LevelConfigGetter>(Lifetime.Singleton);

			_builder.Register<ILevelProgressFacade, LevelProgressFacade>(Lifetime.Singleton);

			#endregion

			#region Repositories

			_builder.Register<GameStateMachineRepository>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

			RegisterProgressEntityRepository();

			_builder.Register<IActiveRepository<IPresenter>, PresentersRepository>(Lifetime.Singleton).AsSelf();
			_builder.Register<IRepository<IView>, ViewsRepository>(Lifetime.Singleton).AsSelf();

			_builder.Register<GameStatesRepositoryInitializer>(Lifetime.Scoped).AsSelf();

			_builder.Register(_ => new LeaderBoardRepository(GetLeaderboard()), Lifetime.Singleton).AsSelf()
				.AsImplementedInterfaces();

			_builder.Register(
				resolver => new ResourcesRepository(
					new ResourceServiceFactory(
						resolver.Resolve<IAssetLoader>().LoadFromResources<UpgradesListConfig>(
							ResourcesAssetPath.Configs.ShopItems
						)
					).CreateIntCurrencies()
				),
				Lifetime.Singleton
			).AsImplementedInterfaces().AsSelf();

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

		private void RegisterCloudSavers()
		{
		}

		private void RegisterLoadingCurtain() =>
			_builder.Register(
				container => new LoadingCurtainFactory(container.Resolve<IAssetLoader>()).Create(),
				Lifetime.Singleton
			).AsImplementedInterfaces().AsSelf();

		private void RegisterProgressEntityRepository()
		{
			_builder.Register(
				resolver =>
				{
					return new ProgressEntityRepository(
						resolver.Resolve<IPersistentProgressService>()
							.GlobalProgress
							.ShopModel
							.ProgressEntities
							.ToDictionary(elem => elem.ConfigId, elem => elem),
						resolver
							.Resolve<IAssetLoader>()
							.LoadFromResources<UpgradesListConfig>(ResourcesAssetPath.Configs.ShopItems).ReadOnlyItems
							.ToDictionary(elem => elem.Id, elem => (IUpgradeEntityViewConfig)elem)
					);
				},
				Lifetime.Singleton
			).AsSelf().AsImplementedInterfaces();
		}
	}
}
