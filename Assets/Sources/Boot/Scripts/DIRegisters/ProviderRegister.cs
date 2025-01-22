using Sources.Boot.Scripts.Factories;
using Sources.Boot.Scripts.Factories.Domain;
using Sources.Boot.UnityApplicationServices;
using Sources.BusinessLogic;
using Sources.BusinessLogic.Services;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.BusinessLogic.ServicesInterfaces.Advertisement;
using Sources.Infrastructure.CoroutineRunner;
using Sources.Utils;
using Sources.Utils.Scene;
using UnityEngine.Audio;
using VContainer;

namespace Sources.Boot.Scripts.DIRegisters
{
	public class ProviderRegister
	{
		private readonly IContainerBuilder _builder;

		public ProviderRegister(IContainerBuilder containerBuilder) =>
			_builder = containerBuilder;

		public void Register()
		{
			_builder.Register(
				resolver =>
				{
					var assetFactory = resolver.Resolve<IAssetLoader>();

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
				resolver => new TranslatorService(new LocalizationService(resolver.Resolve<IAssetLoader>())),
				Lifetime.Singleton
			).AsImplementedInterfaces().AsSelf();

			_builder.Register(
				resolver => resolver.Resolve<IAssetLoader>()
					.LoadFromResources<CoroutineRunner>(ResourcesAssetPath.GameObjects.CoroutineRunner),
				Lifetime.Singleton
			).AsSelf().AsImplementedInterfaces();

			_builder.Register(_ => new CloudPlayerDataServiceFactory().Create(), Lifetime.Singleton).AsSelf()
				.AsImplementedInterfaces();

			_builder.Register(
				resolver => new ResourcePathConfigServiceFactory(resolver.Resolve<IAssetLoader>()).Create(),
				Lifetime.Singleton
			).AsImplementedInterfaces().AsSelf();
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