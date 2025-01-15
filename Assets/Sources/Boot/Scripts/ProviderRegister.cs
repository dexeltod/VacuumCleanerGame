using System;
using Sources.Boot.UnityApplicationServices;
using Sources.BuisenessLogic.Interfaces.Factory.StateMachine;
using Sources.BuisenessLogic.Services;
using Sources.BuisenessLogic.ServicesInterfaces;
using Sources.BuisenessLogic.ServicesInterfaces.Advertisement;
using Sources.Controllers;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.CoroutineRunner;
using Sources.Infrastructure.Factories;
using Sources.Infrastructure.Factories.Domain;
using Sources.Infrastructure.Repository;
using Sources.Infrastructure.Services;
using Sources.Presentation.SceneEntity;
using Sources.Utils;
using Sources.Utils.Scene;
using UnityEngine.Audio;
using VContainer;

namespace Sources.Boot.Scripts
{
	public class ProviderRegister
	{
		private readonly IContainerBuilder _builder;
		private readonly IObjectResolver _resolver;
		private readonly IAssetFactory _assetFactory;

		public ProviderRegister(IObjectResolver resolver, IContainerBuilder containerBuilder, IAssetFactory assetFactory)
		{
			_resolver = resolver;
			_builder = containerBuilder;
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
		}

		public void Register()
		{
			_builder.Register<IGameStateChanger>(
					(resolver) => resolver.Resolve<GameStateChangerFactory>().Create(),
					Lifetime.Scoped
				)
				.AsImplementedInterfaces().AsSelf();

			_builder.Register<SandCarContainerView>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

			_builder.Register<FillMeshShaderControllerProvider>(
				Lifetime.Singleton
			).AsImplementedInterfaces().AsSelf();

			_builder.Register<PlayerModelRepository>(
				Lifetime.Singleton
			).AsImplementedInterfaces().AsSelf();

			_builder.Register<GameFocusHandler>(
				_ =>
				{
					var audioMixer = _assetFactory.LoadFromResources<AudioMixer>(ResourcesAssetPath.GameObjects.AudioMixer);
					var applicationQuitHandler = _assetFactory.InstantiateAndGetComponent<ApplicationQuitHandler>(
						ResourcesAssetPath.GameObjects
							.ApplicationQuitHandler
					);

					return new GameFocusHandler(audioMixer, applicationQuitHandler);
				},
				Lifetime.Singleton
			).AsImplementedInterfaces().AsSelf();

			_builder.Register<SandParticleSystemProvider>(_ => { }
			Lifetime.Singleton
				).AsImplementedInterfaces().AsSelf();

			_builder.Register<CloudServiceSdkFacadeProvider>(
				Lifetime.Singleton
			).AsImplementedInterfaces().AsSelf();

			_builder.Register<AdvertisementPresenter>(_ => new AdvertisementPresenter(RegisterAdvertisement()), Lifetime.Singleton)
				.AsImplementedInterfaces().AsSelf();

			_builder.Register<GameMenuPresenterProvider>(
				Lifetime.Singleton
			).AsImplementedInterfaces().AsSelf();

			_builder.Register<ISaveLoader>(_ => new SaveLoaderFactory().Create(), Lifetime.Singleton).AsImplementedInterfaces()
				.AsSelf();

			_builder.Register<GameplayInterfacePresenterProvider>(
					Lifetime.Singleton
				).AsImplementedInterfaces()
				.AsSelf();

			_builder.Register<UpgradeWindowPresenterProvider>(
				Lifetime.Singleton
			);
			_builder.Register(
				_ => _assetFactory.LoadFromResources<CoroutineRunner>(ResourcesAssetPath.GameObjects.CoroutineRunner),
				Lifetime.Singleton
			).AsSelf().AsImplementedInterfaces();

			_builder.RegisterFactory(() => new CloudPlayerDataServiceFactory().Create());

			_builder.Register<ResourcesProgressPresenterProvider>(
					Lifetime.Singleton
				).AsImplementedInterfaces()
				.AsSelf();

			_builder.Register<DissolveShaderViewController>(
				Lifetime.Singleton
			);

			_builder.Register((_) => new ResourcePathConfigServiceFactory(_assetFactory).Create(), Lifetime.Singleton).;
		}

		private IAdvertisement RegisterAdvertisement()
		{
#if YANDEX_CODE
			return new YandexAdvertisement();
#endif
			return new EditorAdvertisement();
		}
	}
}
