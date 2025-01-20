using Sources.Boot.Scripts.Factories;
using Sources.Boot.Scripts.Factories.Domain;
using Sources.Boot.Scripts.Factories.StateMachine;
using Sources.Boot.UnityApplicationServices;
using Sources.BusinessLogic;
using Sources.BusinessLogic.Services;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.BusinessLogic.ServicesInterfaces.Advertisement;
using Sources.Controllers;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.CoroutineRunner;
using Sources.Infrastructure.Repository;
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

		public ProviderRegister(IContainerBuilder containerBuilder)
		{
			_builder = containerBuilder;
		}

		public void Register()
		{
			_builder.Register<IGameStateChanger>(
					(resolver) => resolver.Resolve<GameStateChangerFactory>().Create(),
					Lifetime.Scoped
				)
				.AsImplementedInterfaces().AsSelf();

			_builder.Register<SandCarContainerView>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

			_builder.Register<FillMeshShaderController>(
				Lifetime.Singleton
			).AsImplementedInterfaces().AsSelf();

			_builder.Register<PlayerModelRepository>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

			_builder.Register(
				resolver =>
				{
					var assetFactory = resolver.Resolve<IAssetFactory>();

					return new GameFocusHandler(
						assetFactory.LoadFromResources<AudioMixer>(ResourcesAssetPath.GameObjects.AudioMixer),
						assetFactory.InstantiateAndGetComponent<ApplicationQuitHandler>(
							ResourcesAssetPath.GameObjects
								.ApplicationQuitHandler
						)
					);
				},
				Lifetime.Singleton
			).AsImplementedInterfaces().AsSelf();

			_builder.Register(
				resolver => new TranslatorService(new LocalizationService(resolver.Resolve<IAssetFactory>())),
				Lifetime.Singleton
			).AsImplementedInterfaces().AsSelf();

			_builder.Register(_ => new AdvertisementPresenter(RegisterAdvertisement()), Lifetime.Singleton)
				.AsImplementedInterfaces().AsSelf();

			_builder.Register<ISaveLoader>(_ => new SaveLoaderFactory().Create(), Lifetime.Singleton).AsImplementedInterfaces()
				.AsSelf();

			_builder.Register(
				resolver =>
				{
					var assetFactory = resolver.Resolve<IAssetFactory>();

					return assetFactory.LoadFromResources<CoroutineRunner>(ResourcesAssetPath.GameObjects.CoroutineRunner);
				},
				Lifetime.Singleton
			).AsSelf().AsImplementedInterfaces();

			_builder.RegisterFactory(() => new CloudPlayerDataServiceFactory().Create());

			_builder.Register(
				(resolver) => new ResourcePathConfigServiceFactory(resolver.Resolve<IAssetFactory>()).Create(),
				Lifetime.Singleton
			);
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
