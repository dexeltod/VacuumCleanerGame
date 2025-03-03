using System.Linq;
using Sources.Boot.Scripts.DIRegisters;
using Sources.Boot.Scripts.Factories;
using Sources.Boot.Scripts.Factories.Domain;
using Sources.Boot.Scripts.Factories.Progress;
using Sources.Boot.Scripts.UpgradeEntitiesConfigs;
using Sources.BusinessLogic;
using Sources.BusinessLogic.Configs;
using Sources.BusinessLogic.Interfaces.Factory;
using Sources.BusinessLogic.Services;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.DomainInterfaces;
using Sources.Infrastructure.CoroutineRunner;
using Sources.Infrastructure.Repository;
using Sources.Infrastructure.Services;
using Sources.Infrastructure.Services.DomainServices;
using Sources.Utils;
using Sources.Utils.Scene;
using UnityEngine.Audio;
using VContainer;

namespace Sources.Boot
{
	public static class ServiceRegisterExtensions
	{
		// public static IContainerBuilder UseVieweEntities(this IContainerBuilder builder)
		// {
		// 	builder.Register<ViewEntity>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
		//
		// 	return builder;
		// }

		public static IContainerBuilder UseAssetLoader(this IContainerBuilder builder)
		{
			builder.Register<IAssetLoader, AssetLoader>(Lifetime.Singleton);

			return builder;
		}

		public static IContainerBuilder UseCoroutineRunner(this IContainerBuilder builder)
		{
			builder
				.Register(
					resolver => resolver
						.Resolve<IAssetLoader>()
						.InstantiateAndGetComponent<CoroutineRunner>(ResourcesAssetPath.GameObjects.CoroutineRunner),
					Lifetime.Singleton
				)
				.AsSelf()
				.AsImplementedInterfaces();

			return builder;
		}

		public static IContainerBuilder UseFactories(this IContainerBuilder builder)
		{
			new FactoriesRegister(builder).Register();

			return builder;
		}

		public static IContainerBuilder UseGameFocusHandler(this IContainerBuilder builder)
		{
			builder
				.Register(
					resolver =>
					{
						var assetFactory = resolver.Resolve<IAssetLoader>();

						return new GameFocusHandler(
							resolver.Resolve<IPersistentProgressService>(),
							assetFactory.LoadFromResources<AudioMixer>(ResourcesAssetPath.GameObjects.AudioMixer),
							assetFactory.InstantiateAndGetComponent<ApplicationQuitHandler>(
								ResourcesAssetPath.GameObjects.ApplicationQuitHandler
							)
						);
					},
					Lifetime.Singleton
				)
				.AsImplementedInterfaces()
				.AsSelf();

			return builder;
		}

		public static IContainerBuilder UseInjectableAssetLoader(this IContainerBuilder builder)
		{
			builder
				.Register<IInjectableAssetLoader, InjectableAssetLoader>(Lifetime.Singleton)
				.As<IInjectableAssetLoader>()
				.AsSelf();

			return builder;
		}

		public static IContainerBuilder UseProgress(this IContainerBuilder builder)
		{
			builder.Register<InitialProgressFactory>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();

			builder
				.Register(
					resolver => new PersistentProgressService(resolver.Resolve<IInitialProgressFactory>().Create()),
					Lifetime.Singleton
				)
				.AsImplementedInterfaces()
				.AsSelf();

			builder.Register<ProgressCleaner>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
			builder.Register<ProgressFactory>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();

			return builder;
		}

		public static IContainerBuilder UseProgressEntityRepository(this IContainerBuilder builder)
		{
			builder
				.Register(
					resolver =>
					{
						return new ProgressEntityRepository(
							resolver
								.Resolve<IPersistentProgressService>()
								.GlobalProgress
								.ShopModel
								.ProgressEntities
								.ToDictionary(elem => elem.ConfigId, elem => elem),
							resolver
								.Resolve<IAssetLoader>()
								.LoadFromResources<UpgradesListConfig>(ResourcesAssetPath.Configs.ShopItems)
								.ReadOnlyItems
								.ToDictionary(elem => elem.Id, elem => (IUpgradeEntityViewConfig)elem)
						);
					},
					Lifetime.Singleton
				)
				.AsSelf()
				.AsImplementedInterfaces();

			return builder;
		}

		public static IContainerBuilder UseSaveLoader(this IContainerBuilder builder)
		{
			builder.Register<IProgressSaveLoadDataService, ProgressSaveLoadDataService>(Lifetime.Singleton);

			builder
				.Register(_ => new SaveLoaderFactory().Create(), Lifetime.Singleton)
				.AsImplementedInterfaces()
				.AsSelf();
			return builder;
		}

		public static IContainerBuilder UseTranslator(this IContainerBuilder builder)
		{
			builder
				.Register(
					resolver => new TranslatorService(new LocalizationService(resolver.Resolve<IAssetLoader>())),
					Lifetime.Singleton
				)
				.AsImplementedInterfaces()
				.AsSelf();
			return builder;
		}
	}
}
