using Sources.Boot.Scripts.Factories;
using Sources.Boot.Scripts.Factories.Domain;
using Sources.Boot.UnityApplicationServices;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.BusinessLogic.ServicesInterfaces.Advertisement;
using VContainer;

namespace Sources.Boot.Scripts.DIRegisters
{
	public class ProviderRegister
	{
		private readonly IContainerBuilder _builder;

		public ProviderRegister(IContainerBuilder containerBuilder) => _builder = containerBuilder;

		public void Register()
		{
			_builder
				.Register(_ => new CloudPlayerDataServiceFactory().Create(), Lifetime.Singleton)
				.AsSelf()
				.AsImplementedInterfaces();

			_builder.Register(
				_ => RegisterAdvertisement(),
				Lifetime.Singleton
			);

			_builder
				.Register(
					resolver => new ResourcePathConfigServiceFactory(resolver.Resolve<IAssetLoader>()).Create(),
					Lifetime.Singleton
				)
				.AsImplementedInterfaces()
				.AsSelf();
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