using System;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces.DTO;
using Sources.ServicesInterfaces;
using VContainer;

namespace Sources.Infrastructure.DataViewModel
{
	public class PlayerProgressProvider : IPlayerProgressProvider
	{
		private const int OnePoint = 1;
		private readonly IGameProgress _playerProgress;
		private readonly IPlayerStatsService _playerStats;

		[Inject]
		public PlayerProgressProvider(IPlayerStatsService playerStats) =>
			_playerStats = playerStats ?? throw new ArgumentNullException(nameof(playerStats));

		public void SetProgress(string progressName)
		{
			IUpgradeProgressData upgradeProgress = _playerProgress.GetByName(progressName);
			int newProgressValue = upgradeProgress.Value + OnePoint;

			_playerStats.Set(upgradeProgress.Name, newProgressValue);
			_playerProgress.SetProgress(progressName, newProgressValue);
		}
	}
}