using System;
using Sources.BusinessLogic.Interfaces.Factory;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Domain.Progress;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Utils.AssetPaths;

namespace Sources.Boot.Scripts.Factories.Domain
{
	public class InitialProgressFactory : IInitialProgressFactory
	{
		private readonly IAssetLoader _assetLoader;
		private readonly IResourcesRepository _resourcesRepository;
		private readonly ShopModelFactory _shopModelFactory;

		public InitialProgressFactory(
			IResourcesRepository resourcesRepository,
			IAssetLoader assetLoader,
			ShopModelFactory shopModelFactory
		)
		{
			_resourcesRepository = resourcesRepository ?? throw new ArgumentNullException(nameof(resourcesRepository));
			_assetLoader = assetLoader ?? throw new ArgumentNullException(nameof(assetLoader));
			_shopModelFactory = shopModelFactory ?? throw new ArgumentNullException(nameof(shopModelFactory));
		}

		public IGlobalProgress Create()
		{
			ShopModel shopModelFactory = _shopModelFactory.LoadList();

			return new GlobalProgress(
				new ResourcesModelFactory(_resourcesRepository).Create(),
				new LevelProgressFactory(
					1,
					GameConfig.DefaultMaxTotalResource
				).Create(),
				shopModelFactory,
				new PlayerModelFactory(_assetLoader, shopModelFactory).Create()
			);
		}
	}
}