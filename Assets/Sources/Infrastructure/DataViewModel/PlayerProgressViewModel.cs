using System;
using Sources.Core.DI;
using Sources.Core.Domain.Progress.Player;
using Sources.DomainServices.Interfaces;
using Sources.Infrastructure.InfrastructureInterfaces;
using Sources.Infrastructure.Services;

namespace Sources.Infrastructure.DataViewModel
{
	public class PlayerProgressViewModel : IPlayerProgressViewModel
	{
		private const int Point = 1;
		private readonly PlayerProgress _playerProgress;
		private readonly IPlayerStatsService _playerStats;

		public PlayerProgressViewModel()
		{
			_playerProgress = ServiceLocator.Container.Get<IPersistentProgressService>().GameProgress
				.PlayerProgress;

			_playerStats = ServiceLocator.Container.Get<IPlayerStatsService>();
		}

		public void SetProgress(string progressName)
		{
			Tuple<string, int> progress = _playerProgress.GetByName(progressName);
			int newProgressValue = progress.Item2 + Point;

			_playerStats.Set(progress.Item1, newProgressValue);
			_playerProgress.ChangeValue(progressName, newProgressValue);
		}
	}
}