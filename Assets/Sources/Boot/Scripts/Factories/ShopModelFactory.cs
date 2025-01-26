using System;
using System.Collections.Generic;
using Sources.Boot.Scripts.UpgradeEntitiesConfigs;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Domain.Progress;
using Sources.Domain.Progress.Entities.Values;
using Sources.InfrastructureInterfaces.Configs;
using Sources.Utils;
using VContainer;

namespace Sources.Boot.Scripts.Factories
{
	public class ShopModelFactory
	{
		private readonly IAssetLoader _assetLoader;

		private List<StatUpgradeEntity> _entities;
		private Dictionary<int, StatUpgradeEntity> _entitiesByName;

		[Inject]
		public ShopModelFactory(IAssetLoader assetFactor) =>
			_assetLoader = assetFactor ?? throw new ArgumentNullException(nameof(assetFactor));

		private string ShopItemsPath => ResourcesAssetPath.Configs.ShopItems;

		public ShopModel LoadList()
		{
			_entities = new List<StatUpgradeEntity>();

			var configs = _assetLoader.LoadFromResources<UpgradesListConfig>(ShopItemsPath);

			foreach (PlayerUpgradeShopConfig config in configs.ReadOnlyItems)
				_entities.Add(
					new StatUpgradeEntity(new IntEntityValue(config.Id, config.Title, 0, config.MaxProgressCount), config.Id)
				);

			return new ShopModel(_entities);
		}
	}
}