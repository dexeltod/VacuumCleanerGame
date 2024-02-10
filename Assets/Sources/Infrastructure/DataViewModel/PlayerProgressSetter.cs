using System;
using Sources.DomainInterfaces;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.DTO;
using VContainer;

namespace Sources.Infrastructure.DataViewModel
{
	public class PlayerProgressSetter : IPlayerProgressProvider
	{
		private const int OnePoint = 1;

		private readonly IPlayerStatsService _playerStats;
		private readonly IPersistentProgressService _progressService;

		private IGameProgress PlayerProgress => _progressService.GameProgress.PlayerProgress;

		[Inject]
		public PlayerProgressSetter(IPlayerStatsService playerStats, IPersistentProgressService progressService)
		{
			_playerStats = playerStats ?? throw new ArgumentNullException(nameof(playerStats));
			_progressService = progressService ?? throw new ArgumentNullException(nameof(progressService));
		}

		public void SetProgress(string progressName)
		{
			IUpgradeProgressData upgradeProgress = PlayerProgress.GetByName(progressName);
			int newProgressValue = upgradeProgress.Value + OnePoint;

			_playerStats.Set(upgradeProgress.Name, newProgressValue);
			PlayerProgress.SetProgress(progressName, newProgressValue);
		}
	}
}