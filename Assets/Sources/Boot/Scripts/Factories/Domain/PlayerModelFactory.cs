using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Boot.Scripts.UpgradeEntitiesConfigs;
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
		private readonly IAssetLoader _assetLoader;
		private readonly ShopModel _shopModelFactory;

		public PlayerModelFactory(IAssetLoader assetLoader, ShopModel shopModelFactory)
		{
			_assetLoader
				= assetLoader ?? throw new ArgumentNullException(nameof(assetLoader));
			_shopModelFactory = shopModelFactory ?? throw new ArgumentNullException(nameof(shopModelFactory));
		}

		private string ShopItems => ResourcesAssetPath.Configs.ShopItems;
		private string StartStats => ResourcesAssetPath.Configs.StartStats;

		public PlayerStatsModel Create() =>
			new(
				InitStats(
					_assetLoader.LoadFromResources<UpgradesListConfig>(ShopItems).ReadOnlyItems,
					_assetLoader.LoadFromResources<StartStatsConfig>(StartStats).Stats
				)
			);

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