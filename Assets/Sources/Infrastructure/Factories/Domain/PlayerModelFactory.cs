using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Domain.Common;
using Sources.Domain.Player;
using Sources.Domain.Progress;
using Sources.Domain.Progress.Entities.Values;
using Sources.Domain.Temp;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Configs;
using Sources.Infrastructure.Factories.UpgradeShop;
using Sources.InfrastructureInterfaces.Providers;
using Sources.InfrastructureInterfaces.Repository;
using Sources.ServicesInterfaces;
using Sources.Utils;

namespace Sources.Infrastructure.Factories.Domain
{
	public class PlayerModelFactory
	{
		private readonly IAssetFactory _assetFactory;
		private readonly ShopModel _shopModelFactory;

		public PlayerModelFactory(IAssetFactory assetFactory, ShopModel shopModelFactory)
		{
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_shopModelFactory = shopModelFactory ?? throw new ArgumentNullException(nameof(shopModelFactory));
		}

		private string ShopItems => ResourcesAssetPath.Configs.ShopItems;
		private string StartStats => ResourcesAssetPath.Configs.StartStats;

		public PlayerModel Create()
		{
			List<Stat> stats = new List<Stat>();

			var startConfigs = _assetFactory.LoadFromResources<StartStatsConfig>(StartStats).Stats;

			var items = _assetFactory.LoadFromResources<UpgradeEntityListConfig>(ShopItems)
				.ReadOnlyItems;

			InitStats(items, startConfigs, ref stats);

			return new PlayerModel(stats);
		}

		private void InitStats(
			IReadOnlyCollection<UpgradeEntityViewConfig> items,
			IReadOnlyCollection<StartStatConfig> startConfigs,
			ref List<Stat> stats
		)
		{
			for (int i = 0; i < items.Count; i++)
			{
				var progress = GetProgress(i);

				float startValue = GetStartValuesFromStartConfig(startConfigs, configIndex: i);

				int id = startConfigs.ElementAt(i).Id;
				stats.Add(new Stat(startValue, progress as IntEntityValue, id));
			}
		}

		private float GetStartValuesFromStartConfig(
			IReadOnlyCollection<StartStatConfig> startConfigs,
			int configIndex
		) =>
			startConfigs.ElementAt(configIndex).Stat;

		private IReadOnlyProgressValue<int> GetProgress(int i) =>
			_shopModelFactory.ProgressEntities.ElementAt(i).CurrentLevel ??
			throw new ArgumentNullException($"shopModelFactory.ProgressEntities.ElementAt({i}).CurrentLevel is null");

		private UpgradeEntityViewConfig GetConfigElement(IEnumerable<UpgradeEntityViewConfig> items, int i) =>
			items.ElementAt(i) ??
			throw new ArgumentNullException($"items.ElementAt({i}) is null");
	}
}