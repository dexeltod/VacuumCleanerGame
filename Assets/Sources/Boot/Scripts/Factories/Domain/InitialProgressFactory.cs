using System;
using Sources.BusinessLogic.Interfaces.Factory;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Domain.Progress;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Utils.AssetPaths;
using VContainer;

namespace Sources.Boot.Scripts.Factories.Domain
{
	public class InitialProgressFactory : IInitialProgressFactory
	{
		private readonly IAssetLoader _assetLoader;
		private readonly IResourcesRepository _resourcesRepository;

		[Inject]
		public InitialProgressFactory(
			IResourcesRepository resourcesRepository,
			IAssetLoader assetLoader
		)
		{
			_resourcesRepository = resourcesRepository ?? throw new ArgumentNullException(nameof(resourcesRepository));
			_assetLoader = assetLoader ?? throw new ArgumentNullException(nameof(assetLoader));
		}

		public IGlobalProgress Create() =>
			new GlobalProgress(
				new ResourcesModelFactory(_resourcesRepository).Create(),
				new LevelProgressFactory(
					1,
					GameConfig.DefaultMaxTotalResource
				).Create(),
				new ShopModelFactory(_assetLoader).LoadList(),
				new PlayerModelFactory(_assetLoader, new ShopModelFactory(_assetLoader).LoadList()).Create()
			);
	}
}
