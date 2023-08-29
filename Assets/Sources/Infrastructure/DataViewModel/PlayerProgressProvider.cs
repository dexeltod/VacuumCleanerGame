using Sources.DIService;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces.DTO;
using Sources.ServicesInterfaces;

namespace Sources.Infrastructure.DataViewModel
{
	public class PlayerProgressProvider : IPlayerProgressProvider
	{
		private const int Point = 1;
		private readonly IGameProgress _playerProgress;
		private readonly IPlayerStatsService _playerStats;

		public PlayerProgressProvider()
		{
			_playerProgress = GameServices.Container.Get<IPersistentProgressService>().GameProgress
				.PlayerProgress;

			_playerStats = GameServices.Container.Get<IPlayerStatsService>();
		}

		public void SetProgress(string progressName)
		{
			IUpgradeProgressData upgradeProgress = _playerProgress.GetByName(progressName);
			int newProgressValue = upgradeProgress.Value + Point;

			_playerStats.Set(upgradeProgress.Name, newProgressValue);
			_playerProgress.SetProgress(progressName, newProgressValue);
		}
	}
}