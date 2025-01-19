using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Boot.Scripts.Factories.UpgradeEntitiesConfigs;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Domain.Player;
using Sources.Domain.Progress;
using Sources.Domain.Progress.Entities.Values;
using Sources.Domain.Stats;
using Sources.DomainInterfaces.DomainServicesInterfaces;
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
			List<Stat> stats = new List<Stat>();

			IReadOnlyCollection<StartStatConfig> startConfigs =
				_assetFactory.LoadFromResources<StartStatsConfig>(StartStats).Stats;

			var items = _assetFactory.LoadFromResources<UpgradesListConfig>(ShopItems)
				.ReadOnlyItems;

			var myStats = items.Select(elem => new Stat(0, new IntEntityValue(elem.Id), elem.Id));

			InitStats(items, startConfigs, ref stats);

			return new PlayerStatsModel(stats);
		}

		private void InitStats(
			IReadOnlyCollection<PlayerUpgradeShopConfig> items,
			IReadOnlyCollection<StartStatConfig> startConfigs,
			ref List<Stat> stats
		)
		{
			var myStats = items.Select(elem => new Stat())

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

		private IReadOnlyProgress<int> GetProgress(int i) =>
			_shopModelFactory.ProgressEntities.ElementAt(i).LevelProgress ??
			throw new ArgumentNullException($"shopModelFactory.ProgressEntities.ElementAt({i}).LevelProgress is null");
	}
}
