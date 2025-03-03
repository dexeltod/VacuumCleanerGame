using System;
using System.Collections.Generic;
using Sources.Boot.Scripts.Factories.Domain;
using Sources.Boot.Scripts.UpgradeEntitiesConfigs;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Services.DomainServices;
using Sources.Utils;

namespace Sources.Boot.Scripts.Factories.Repositories
{
	public class ResourceRepositoryFactory
	{
		private readonly IAssetLoader _assetLoader;
		private readonly ILevelProgress _levelProgress;

		public ResourceRepositoryFactory(IAssetLoader assetLoader, ILevelProgress levelProgress)
		{
			_assetLoader = assetLoader;
			_levelProgress =
				levelProgress ?? throw new ArgumentNullException(nameof(levelProgress));
		}

		public ResourcesRepository Create()
		{
			string shopItems = ResourcesAssetPath.Configs.ShopItems;

			Dictionary<int, IResource<int>> resources = new ResourceFactory(
				_assetLoader.LoadFromResources<UpgradesListConfig>(shopItems),
				_levelProgress
			).CreateIntResource();

			return new ResourcesRepository(resources);
		}
	}
}
