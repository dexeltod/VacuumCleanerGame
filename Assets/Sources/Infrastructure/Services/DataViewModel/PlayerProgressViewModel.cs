using System;
using Application.DI;
using Domain.Progress.Player;
using InfrastructureInterfaces;

namespace Infrastructure.Services.DataViewModel
{
	public class PlayerProgressViewModel : IPlayerProgressViewModel
	{
		private const int Point = 1;
		private readonly PlayerProgress _playerProgress;
		private readonly IPlayerStatsService _playerStats;

		public PlayerProgressViewModel()
		{
			_playerProgress = ServiceLocator.Container.GetSingle<IPersistentProgressService>().GameProgress
				.PlayerProgress;

			_playerStats = ServiceLocator.Container.GetSingle<IPlayerStatsService>();
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