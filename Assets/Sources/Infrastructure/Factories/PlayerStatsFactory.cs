using System.Collections.Generic;
using Sources.Domain;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces.Factory;
using Sources.Services;
using Sources.Services.PlayerServices;
using Sources.ServicesInterfaces.Upgrade;
using VContainer;

namespace Sources.Infrastructure.Factories
{
	public class PlayerStatsFactory
	{
		private readonly IProgressUpgradeFactory _shopFactory;
		private PlayerStatsService _playerStatsService;

		[Inject]
		public PlayerStatsFactory(IProgressUpgradeFactory progressUpgradeFactory) =>
			_shopFactory = progressUpgradeFactory;

		public PlayerStatsService CreatePlayerStats(IPersistentProgressService persistentProgressService)
		{
			if (_playerStatsService != null)
				return _playerStatsService;

			IUpgradeItemData[] items = _shopFactory.LoadItems();

			Dictionary<string, int[]> stats = CreateStatsDictionary(items);

			List<IUpgradeProgressData> progress = persistentProgressService.GameProgress.PlayerProgress.GetAll();

			string[] statNames = new string[progress.Count];
			IPlayerStatChangeable[] playerStats = new IPlayerStatChangeable[progress.Count];

			ShopPointsToStatsConverter converter = new ShopPointsToStatsConverter(stats);
			InitArrays(progress, statNames, playerStats, converter);

			_playerStatsService = new PlayerStatsService(statNames, playerStats, progress, converter);

			return _playerStatsService;
		}

		private Dictionary<string, int[]> CreateStatsDictionary(IUpgradeItemData[] upgradeItemData)
		{
			Dictionary<string, int[]> stats = new Dictionary<string, int[]>();
			{
				foreach (IUpgradeItemData stat in upgradeItemData)
					stats.Add(stat.IdName, stat.Stats);
			}

			return stats;
		}

		private void InitArrays(
			List<IUpgradeProgressData> progress,
			string[] statNames,
			IPlayerStatChangeable[] playerStats,
			ShopPointsToStatsConverter shopPointsToStatsConverter
		)
		{
			CreateNames(progress, statNames);
			CreateStats(progress, playerStats, shopPointsToStatsConverter);
		}

		private void CreateStats(
			List<IUpgradeProgressData> progress,
			IPlayerStatChangeable[] playerStats,
			ShopPointsToStatsConverter converter
		)
		{
			for (int i = 0; i < progress.Count; i++)
				playerStats[i] = new PlayerStat(
					progress[i].Name,
					converter.GetConverted(
						progress[i].Name,
						progress[i].Value
					)
				);
		}

		private void CreateNames(List<IUpgradeProgressData> progress, string[] statNames)
		{
			for (int i = 0; i < progress.Count; i++)
				statNames[i] = progress[i].Name;
		}
	}
}