using System;
using System.Collections.Generic;
using System.Linq;
using Sources.DomainInterfaces;
using Sources.ServicesInterfaces;

namespace Sources.Services.PlayerServices
{
	public sealed class PlayerStatsService : IPlayerStatsService
	{
		public const string Speed = "Speed";
		public const string ScoreCash = "ScoreCash";

		private readonly ShopPointsToStatsConverter _converter;

		private readonly List<IUpgradeProgressData> _progress;
		private readonly string[] _statNames;
		private readonly IPlayerStatChangeable[] _playerStats;

		public PlayerStatsService(
			string[] statNames,
			IPlayerStatChangeable[] playerStats,
			List<IUpgradeProgressData> progress,
			ShopPointsToStatsConverter statsConverter
		)
		{
			_statNames = statNames ?? throw new ArgumentNullException(nameof(statNames));
			_playerStats = playerStats ?? throw new ArgumentNullException(nameof(playerStats));
			_progress = progress ?? throw new ArgumentNullException(nameof(progress));
			_converter = statsConverter ?? throw new ArgumentNullException(nameof(statsConverter));
		}

		public IPlayerStatReadOnly Get(string progressName)
		{
			if (_statNames.Contains(progressName) == false)
				throw new InvalidOperationException("The progress name " + progressName + "not exists");

			IUpgradeProgressData progress = _progress.FirstOrDefault(elem => elem.Name == progressName);

			if (progress == null)
				throw new NullReferenceException("Progress is null");

			return _playerStats.FirstOrDefault(elem => elem.Name == progressName);
		}

		public void Set(string progressName, int value)
		{
			if (_statNames.Contains(progressName) == false)
				throw new InvalidOperationException("The progress name " + progressName + "not exists");
			
			if (value < 0)
				throw new ArgumentOutOfRangeException(nameof(value));

			IUpgradeProgressData progress = _progress.FirstOrDefault(elem => elem.Name == progressName);
			IPlayerStatChangeable stat = _playerStats.FirstOrDefault(elem => elem.Name == progressName);

			if (progress == null)
				throw new NullReferenceException($"Progress {progressName} is not found");
			if (stat == null)
				throw new NullReferenceException($"Stat {progressName} is not found");

			stat.SetValue(_converter.GetConverted(progressName, value));
			progress.Value = value;
		}
	}
}