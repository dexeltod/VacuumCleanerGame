using System;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces;
using VContainer;

namespace Sources.Infrastructure
{
	public class LevelProgressFacade : ILevelProgressFacade
	{
		private const int OnePoint = 1;

		private readonly ProgressConstantNames _progressConstantNames;
		private readonly IGameProgress _progressService;

		public int CurrentLevelNumber { get; private set; }
		private string CurrentLevel => _progressConstantNames.CurrentLevel;

		[Inject]
		public LevelProgressFacade(
			IPersistentProgressService progressService,
			ProgressConstantNames progressConstantNames
		)
		{
			if (progressService == null) throw new ArgumentNullException(nameof(progressService));
			_progressConstantNames
				= progressConstantNames ?? throw new ArgumentNullException(nameof(progressConstantNames));

			_progressService = progressService.GameProgress.LevelProgress;
			CurrentLevelNumber = _progressService.GetByName(CurrentLevel).Value;
		}

		public void SetNextLevel()
		{
			IUpgradeProgressData progressData = _progressService.GetByName(CurrentLevel);

			int newLevel = progressData.Value + OnePoint;
			CurrentLevelNumber = newLevel;

			_progressService.SetProgress(CurrentLevel, newLevel);
		}
	}
}