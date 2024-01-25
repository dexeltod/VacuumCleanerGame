using System;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces;
using VContainer;

namespace Sources.Infrastructure
{
	public class LevelProgressFacade : ILevelProgressFacade
	{
		private const string CurrentLevelName = "Desert";
		private const int OnePoint = 1;

		private readonly IGameProgress _progressService;

		public int CurrentLevelNumber { get; private set; }

		[Inject]
		public LevelProgressFacade(IPersistentProgressService progressService)
		{
			if (progressService == null) throw new ArgumentNullException(nameof(progressService));
			
			_progressService = progressService.GameProgress.LevelProgress;
			CurrentLevelNumber = _progressService.GetByName(CurrentLevelName).Value;
		}

		public void SetNextLevel()
		{
			IUpgradeProgressData progressData = _progressService.GetByName(CurrentLevelName);

			int newLevel = progressData.Value + OnePoint;
			CurrentLevelNumber = newLevel;

			_progressService.SetProgress(CurrentLevelName, newLevel);
		}
	}
}