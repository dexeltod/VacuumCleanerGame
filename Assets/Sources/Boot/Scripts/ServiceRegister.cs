using System;
using System.Linq;
using Sources.Boot.Scripts.Factories;
using Sources.Boot.Scripts.Factories.Domain;
using Sources.Boot.Scripts.Factories.Presentation.Scene;
using Sources.Boot.Scripts.Factories.StateMachine;
using Sources.Boot.Scripts.Factories.UpgradeEntitiesConfigs;
using Sources.BusinessLogic;
using Sources.BusinessLogic.Interfaces;
using Sources.BusinessLogic.Interfaces.Configs;
using Sources.BusinessLogic.Scene;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Controllers.Services;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Leaderboard;
using Sources.Infrastructure.Repository;
using Sources.Infrastructure.Services;
using Sources.Infrastructure.Services.DomainServices;
using Sources.InfrastructureInterfaces.Configs;
using Sources.InfrastructureInterfaces.Configs.Scripts;
using Sources.Utils;
using UnityEngine;
using VContainer;
using VContainer.Unity;
#if YANDEX_CODE
#endif

namespace Sources.Boot.Scripts
{
	public class ServiceRegister
	{
		private readonly IContainerBuilder _builder;
		private readonly IObjectResolver _resolver;

		public ServiceRegister(IContainerBuilder builder)
		{
			_builder = builder ?? throw new ArgumentNullException(nameof(builder));
		}

		public void Register()
		{
			_builder.RegisterEntryPoint<Boot>();

			#region BaseServices

			_builder.Register<GameBuilder>(Lifetime.Singleton);
			_builder.Register<IInjectableAssetFactory, InjectableAssetFactory>(Lifetime.Singleton).As<IInjectableAssetFactory>()
				.AsSelf();

			_builder.Register<IAssetFactory, AssetFactory>(Lifetime.Singleton);
			_builder.Register<ISceneLoader, SceneLoader>(Lifetime.Singleton);
			_builder.Register<ILocalizationService, LocalizationService>(Lifetime.Singleton);

			_builder.Register<ProgressionConfig>(Lifetime.Singleton);

			_builder.Register<ClearProgressFactory>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

			_builder.Register(
				resolver =>
				{
					IGlobalProgress progress = resolver.Resolve<ClearProgressFactory>().Create();
					var service = new UpdatablePersistentProgressService();
					service.Update(progress);
					return service;
				},
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

			_builder.Register<GameStateChangerFactory>(Lifetime.Scoped).AsImplementedInterfaces();

			#region Factories

			new FactoriesRegister(_builder).Register();

			#endregion

			#region States

			_builder.Register(
				resolver => new GameStateChangerFactory(new GameStatesRepositoryFactory(resolver)).Create(),
				Lifetime.Singleton
			);

			#endregion

			#region InitializeServicesAndProgress

			_builder.Register<ILevelChangerService, LevelChangerService>(Lifetime.Singleton);

			_builder.Register<IProgressSaveLoadDataService, ProgressSaveLoadDataService>(Lifetime.Singleton);

			RegisterCloudSavers();

			_builder.Register(_ => new SaveLoaderFactory().Create(), Lifetime.Singleton)
				.AsImplementedInterfaces().AsSelf();

			CreateSceneLoadServices();

			#endregion

			#region InitializeProgressServices

			_builder.Register<ILevelConfigGetter, LevelConfigGetter>(Lifetime.Singleton);

			_builder.Register<ILevelProgressFacade, LevelProgressFacade>(Lifetime.Singleton);

			#endregion

			#region Repositories

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
							.Resolve<IAssetFactory>()
							.LoadFromResources<UpgradesListConfig>(ResourcesAssetPath.Configs.ShopItems).ReadOnlyItems
							.ToDictionary(elem => elem.Id, elem => (IUpgradeEntityConfig)elem)
					);
				},
				Lifetime.Singleton
			).AsSelf().AsImplementedInterfaces();

			_builder.Register<GameStatesRepositoryFactory>(Lifetime.Scoped);
			_builder.Register<PresentersContainerRepository>(Lifetime.Scoped).AsSelf().AsImplementedInterfaces();
			_builder.RegisterInstance<ILeaderBoardService>(new LeaderBoardRepository(GetLeaderboard()));
			_builder.RegisterInstance<IResourcesRepository>(
				new ResourcesRepository(new ResourceServiceFactory().CreateIntCurrencies())
			);

			#endregion

			_builder.RegisterEntryPointExceptionHandler(
				exception => Debug.LogError(exception.Message)
			);
		}

		private void RegisterCloudSavers()
		{
		}

		private void RegisterLoadingCurtain() =>
			_builder.Register(
				container => container.Resolve<LoadingCurtainFactory>().Create(),
				Lifetime.Singleton
			).AsImplementedInterfaces().AsSelf();

		private void CreateSceneLoadServices()
		{
			ServicesLoadInvokerInformer servicesLoadInvokerInformer = new ServicesLoadInvokerInformer();

			_builder.RegisterInstance<ISceneLoadInformer>(
				servicesLoadInvokerInformer
			);
			_builder.RegisterInstance<ISceneLoadInvoker>(
				servicesLoadInvokerInformer
			);
		}

		private IAbstractLeaderBoard GetLeaderboard()
		{
#if YANDEX_CODE
			return new YandexLeaderboard();
#endif
			return new TestLeaderBoardService();
		}
	}
}
