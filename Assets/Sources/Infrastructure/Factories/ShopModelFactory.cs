using System;
using System.Collections.Generic;
using Sources.Domain.Progress;
using Sources.Domain.Progress.Entities.Values;
using Sources.Infrastructure.Factories.UpgradeEntitiesConfigs;
using Sources.Infrastructure.Services;
using Sources.InfrastructureInterfaces.Configs;
using Sources.Utils;

namespace Sources.Infrastructure.Factories
{
	public class ShopModelFactory
	{
		private readonly AssetFactory _assetFactory;

		private List<StatUpgradeEntity> _entities;
		private Dictionary<int, StatUpgradeEntity> _entitiesByName;

		public ShopModelFactory(AssetFactory assetFactor) =>
			_assetFactory
				= assetFactor ?? throw new ArgumentNullException(nameof(assetFactor));

		private string ShopItems => ResourcesAssetPath.Configs.ShopItems;

		public ShopModel LoadList()
		{
			_entities = new List<StatUpgradeEntity>();

			UpgradesListConfig configs = _assetFactory.LoadFromResources<UpgradesListConfig>(ShopItems);

			foreach (PlayerUpgradeShopViewsConfig config in configs.ReadOnlyItems)
				_entities.Add(new StatUpgradeEntity(new IntEntityValue(0), config.Id));

			return new ShopModel(_entities);
		}
	}
}
