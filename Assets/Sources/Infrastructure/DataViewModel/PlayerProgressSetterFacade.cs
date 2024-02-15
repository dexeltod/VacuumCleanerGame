using System;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces.Providers;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.DTO;

namespace Sources.Infrastructure.DataViewModel
{
	public class PlayerProgressSetterFacade : IPlayerProgressSetterFacade
	{
		private const int OnePoint = 1;

		private readonly IPlayerStatsServiceProvider _playerStats;
		private readonly IPersistentProgressService _persistentProgressService;

		private IGameProgress PlayerProgress => _persistentProgressService.GameProgress.PlayerProgress;
	
		public PlayerProgressSetterFacade(
			IPlayerStatsServiceProvider playerStats,
			IPersistentProgressService persistentProgressService
		)
		{
			_playerStats = playerStats ?? throw new ArgumentNullException(nameof(playerStats));
			_persistentProgressService = persistentProgressService ??
				throw new ArgumentNullException(nameof(persistentProgressService));
		}

		public bool TryAddOneProgressPoint(string progressName)
		{
			IUpgradeProgressData upgradeProgress = PlayerProgress.GetByName(progressName);

			int newProgressValue = upgradeProgress.Value + OnePoint;

			if (newProgressValue >= PlayerProgress.MaxUpgradePointCount)
				return false;

			_playerStats.Implementation.Set(upgradeProgress.Name, newProgressValue);
			PlayerProgress.SetProgress(progressName, newProgressValue);

			return true;
		}
	}
}