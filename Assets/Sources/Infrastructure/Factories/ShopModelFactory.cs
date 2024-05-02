using System;
using System.Collections.Generic;
using Sources.Domain.Progress;
using Sources.Domain.Progress.Entities.Values;
using Sources.Infrastructure.Configs;
using Sources.Infrastructure.Factories.UpgradeEntitiesConfigs;
using Sources.ServicesInterfaces;
using Sources.Utils;

namespace Sources.Infrastructure.Factories
{
	public class ShopModelFactory
	{
		private readonly IAssetFactory _assetFactory;

		private List<UpgradeEntity> _entities;
		private Dictionary<int, UpgradeEntity> _entitiesByName;

		public ShopModelFactory(IAssetFactory assetFactor) =>
			_assetFactory = assetFactor ?? throw new ArgumentNullException(nameof(assetFactor));

		private string ShopItems => ResourcesAssetPath.Configs.ShopItems;

		public ShopModel LoadList()
		{
			_entities = new List<UpgradeEntity>();

			UpgradeEntityListConfig configs = _assetFactory.LoadFromResources<UpgradeEntityListConfig>
				(ShopItems);

			foreach (UpgradeEntityViewConfig config in configs.ReadOnlyItems)
				_entities.Add(new UpgradeEntity(new IntEntityValue(0), config.Id));

			return new ShopModel(_entities);
		}
	}
}