using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Boot.Scripts.Factories.UpgradeEntitiesConfigs;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Domain.Player;
using Sources.Domain.Progress;
using Sources.Domain.Progress.Entities.Values;
using Sources.Domain.Stats;
using Sources.InfrastructureInterfaces.Configs;
using Sources.Utils;

namespace Sources.Boot.Scripts.Factories.Domain
{
	public class PlayerModelFactory
	{
		private readonly IAssetFactory _assetFactory;
		private readonly ShopModel _shopModelFactory;

		public PlayerModelFactory(IAssetFactory assetFactory, ShopModel shopModelFactory)
		{
			_assetFactory
				= assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_shopModelFactory = shopModelFactory ?? throw new ArgumentNullException(nameof(shopModelFactory));
		}

		private string ShopItems => ResourcesAssetPath.Configs.ShopItems;
		private string StartStats => ResourcesAssetPath.Configs.StartStats;

		public PlayerStatsModel Create()
		{
			return new PlayerStatsModel(
				InitStats(
					_assetFactory.LoadFromResources<UpgradesListConfig>(ShopItems).ReadOnlyItems,
					_assetFactory.LoadFromResources<StartStatsConfig>(StartStats).Stats
				)
			);
		}

		private List<Stat> InitStats(
			IReadOnlyCollection<PlayerUpgradeShopConfig> items,
			IReadOnlyCollection<StartStatConfig> startConfigs
		)
		{
			return items.Join(
				startConfigs,
				item => item.Id,
				start => (int)start.Type,
				(item, start) => new Stat(start.Stat, new IntEntityValue(item.Id, item.Title, 0), item.Id)
			).ToList();
		}
	}
}