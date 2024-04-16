using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Domain.Stats;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Common.Factory;
using Sources.Infrastructure.Configs;
using Sources.Infrastructure.Factories.UpgradeShop;
using Sources.Infrastructure.ScriptableObjects;
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
		private readonly ProgressEntityFactory _shopFactory;
		private readonly IPersistentProgressServiceProvider _persistentProgressService;
		private readonly IPlayerStatsServiceProvider _playerStatsServiceProvider;

		private PlayerStatsService _playerStatsService;

		[Inject]
		public PlayerStatsFactory(
			ProgressEntityFactory progressEntityFactory,
			IPersistentProgressServiceProvider persistentProgressService,
			IPlayerStatsServiceProvider playerStatsServiceProvider
		)
		{
			_shopFactory = progressEntityFactory ?? throw new ArgumentNullException(nameof(progressEntityFactory));
			_persistentProgressService = persistentProgressService ??
				throw new ArgumentNullException(nameof(persistentProgressService));
			_playerStatsServiceProvider = playerStatsServiceProvider ??
				throw new ArgumentNullException(nameof(playerStatsServiceProvider));
		}

		private IGameProgress ShopProgress =>
			_persistentProgressService.Implementation.GlobalProgress.UpgradeProgressModel;

		private IGlobalProgress GlobalProgressPlayerProgress =>
			_persistentProgressService.Implementation.GlobalProgress;

		public override IPlayerStatsService Create()
		{
			List<IUpgradeProgressData> progress = ShopProgress.GetAll();
			int progressCount = progress.Count;

			string[] statNames = new string[progressCount];
			IPlayerStatChangeable[] playerStats = new IPlayerStatChangeable[progressCount];

			IReadOnlyCollection<UpgradeEntityViewConfig> entities = _shopFactory.Load();

			Dictionary<string, int[]> stats = CreateStatsDictionary(entities);

			ShopPointsToStatsConverter converter = new ShopPointsToStatsConverter(stats);

			FillArrays(progress, statNames, playerStats, converter);

			_playerStatsService = new PlayerStatsService(statNames, playerStats, progress, converter);

			_playerStatsServiceProvider.Register<IPlayerStatsService>(_playerStatsService);
			return _playerStatsService;
		}

		private Dictionary<string, int[]> CreateStatsDictionary(IEnumerable<UpgradeEntityViewConfig> upgradeItemData) =>
			upgradeItemData
				.Cast<IUpgradeItemData>()
				.ToDictionary(stat => stat.IdName, stat => stat.Stats);

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