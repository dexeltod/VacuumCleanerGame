using System;
using System.Collections.Generic;
using Sources.Domain.Progress;
using Sources.Domain.Temp;
using Sources.Infrastructure.Configs;
using Sources.Infrastructure.Factories.UpgradeShop;
using Sources.ServicesInterfaces;
using Sources.Utils;

namespace Sources.Infrastructure.Factories.Player
{
	public class ShopEntityFactory
	{
		private readonly IAssetFactory _assetFactory;

		private List<ProgressEntity> _entities;
		private Dictionary<int, ProgressEntity> _entitiesByName;

		public ShopEntityFactory(IAssetFactory assetFactor) =>
			_assetFactory = assetFactor ?? throw new ArgumentNullException(nameof(assetFactor));

		private string ShopItems => ResourcesAssetPath.Configs.ShopItems;

		public ShopEntity LoadList()
		{
			_entities = new List<ProgressEntity>();

			UpgradeEntityListConfig configs = _assetFactory.LoadFromResources<UpgradeEntityListConfig>
				(ShopItems);

			foreach (UpgradeEntityViewConfig config in configs.ReadOnlyItems)
			{
				_entities.Add(new ProgressEntity(1, config.Id));
			}

			return new ShopEntity(_entities);
		}
	}
}