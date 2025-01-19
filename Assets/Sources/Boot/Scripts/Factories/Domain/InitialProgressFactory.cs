using System;
using Sources.BusinessLogic.Interfaces.Factory;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Domain.Progress;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Repository;
using Sources.Infrastructure.Services.DomainServices;
using Sources.Utils.AssetPaths;

namespace Sources.Boot.Scripts.Factories.Domain
{
	public class InitialProgressFactory : IInitialProgressFactory
	{
		private readonly IResourcesRepository _resourcesRepository;
		private readonly IAssetFactory _assetFactory;
		private readonly ShopModelFactory _shopModelFactory;

		public InitialProgressFactory(
			IResourcesRepository resourcesRepository,
			UpdatablePersistentProgressService updatablePersistentProgressServiceProvider,
			ProgressEntityRepository progressEntityRepositoryProvider,
			IAssetFactory assetFactory,
			ShopModelFactory shopModelFactory
		)
		{
			_resourcesRepository = resourcesRepository ?? throw new ArgumentNullException(nameof(resourcesRepository));
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_shopModelFactory = shopModelFactory ?? throw new ArgumentNullException(nameof(shopModelFactory));
		}

		public IGlobalProgress Create()
		{
			ShopModel shopModelFactory = _shopModelFactory.LoadList();

			return new GlobalProgress(
				new ResourcesModelFactory(_resourcesRepository).Create(),
				new LevelProgressFactory(
					firstLevel: 1,
					GameConfig.DefaultMaxTotalResource
				).Create(),
				shopModelFactory,
				new PlayerModelFactory(_assetFactory, shopModelFactory).Create()
			);
		}
	}
}