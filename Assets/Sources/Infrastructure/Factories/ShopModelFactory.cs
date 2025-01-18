using System;
using System.Collections.Generic;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Domain.Progress;
using Sources.Domain.Progress.Entities.Values;
using Sources.Infrastructure.Factories.UpgradeEntitiesConfigs;
using Sources.InfrastructureInterfaces.Configs;
using Sources.Utils;

namespace Sources.Infrastructure.Factories
{
	public class ShopModelFactory
	{
		private readonly IAssetFactory _assetFactory;

		private List<StatUpgradeEntity> _entities;
		private Dictionary<int, StatUpgradeEntity> _entitiesByName;

		public ShopModelFactory(IAssetFactory assetFactor) =>
			_assetFactory = assetFactor ?? throw new ArgumentNullException(nameof(assetFactor));

		private string ShopItemsPath => ResourcesAssetPath.Configs.ShopItems;

		public ShopModel LoadList()
		{
			_entities = new List<StatUpgradeEntity>();

			UpgradesListConfig configs = _assetFactory.LoadFromResources<UpgradesListConfig>(ShopItemsPath);

			foreach (PlayerUpgradeShopViewsConfig config in configs.ReadOnlyItems)
				_entities.Add(new StatUpgradeEntity(new IntEntityValue(config.Id, config.Title, 0), config.Id));

			return new ShopModel(_entities);
		}
	}
}
