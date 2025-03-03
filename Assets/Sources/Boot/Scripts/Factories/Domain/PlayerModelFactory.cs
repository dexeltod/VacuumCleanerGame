using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Boot.Scripts.UpgradeEntitiesConfigs;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Domain.Player;
using Sources.Domain.Progress;
using Sources.Domain.Progress.Entities.Values;
using Sources.Domain.Stats;
using Sources.Infrastructure.Configs;
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
			IReadOnlyCollection<PlayerUpgradeShopViewConfig> items,
			IReadOnlyCollection<StartStatConfig> startConfigs
		)
		{
			List<Stat> result = items
				.Join(
					startConfigs,
					item => item.Id,
					start => start.Id,
					(item, start) => new Stat(start.Stat, new FloatEntity(item.Id, item.Title, 0, item.MaxProgressCount), item.Id)
				)
				.ToList();

			return result;
		}
	}
}