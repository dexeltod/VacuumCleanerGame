using System;
using System.Collections.Generic;
using Sources.Domain.Stats;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Common.Factory;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Providers;
using Sources.Services;
using Sources.Services.PlayerServices;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.Upgrade;
using VContainer;

namespace Sources.Infrastructure.Factories.Player
{
	public class PlayerStatsFactory : Factory<IPlayerStatsService>
	{
		private readonly IProgressUpgradeFactory _shopFactory;
		private readonly IPersistentProgressServiceProvider _persistentProgressService;
		private readonly IPlayerStatsServiceProvider _playerStatsServiceProvider;

		private PlayerStatsService _playerStatsService;

		[Inject]
		public PlayerStatsFactory(
			IProgressUpgradeFactory progressUpgradeFactory,
			IPersistentProgressServiceProvider persistentProgressService,
			IPlayerStatsServiceProvider playerStatsServiceProvider
		)
		{
			_shopFactory = progressUpgradeFactory ?? throw new ArgumentNullException(nameof(progressUpgradeFactory));
			_persistentProgressService = persistentProgressService ??
				throw new ArgumentNullException(nameof(persistentProgressService));
			_playerStatsServiceProvider = playerStatsServiceProvider ??
				throw new ArgumentNullException(nameof(playerStatsServiceProvider));
		}

		private IGameProgress ShopProgress => _persistentProgressService.Implementation.GlobalProgress.ShopProgress;

		private IGlobalProgress GlobalProgressPlayerProgress =>
			_persistentProgressService.Implementation.GlobalProgress;

		public override IPlayerStatsService Create()
		{
			List<IUpgradeProgressData> progress = ShopProgress.GetAll();
			int progressCount = progress.Count;

			string[] statNames = new string[progressCount];
			IPlayerStatChangeable[] playerStats = new IPlayerStatChangeable[progressCount];

			IUpgradeItemData[] items = _shopFactory.LoadItems();

			Dictionary<string, int[]> stats = CreateStatsDictionary(items);

			ShopPointsToStatsConverter converter = new ShopPointsToStatsConverter(stats);
			FillArrays(progress, statNames, playerStats, converter);

			_playerStatsService = new PlayerStatsService(statNames, playerStats, progress, converter);

			_playerStatsServiceProvider.Register<IPlayerStatsService>(_playerStatsService);
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

		private void FillArrays(
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