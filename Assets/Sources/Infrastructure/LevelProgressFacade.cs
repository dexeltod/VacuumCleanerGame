using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces;

namespace Sources.Infrastructure
{
	public class LevelProgressFacade : ILevelProgressFacade
	{
		private const string CurrentLevelName = "CurrentLevel";
		private const int OnePoint = 1;

		private readonly IGameProgress _progressService;

		public int CurrentLevelNumber { get; private set; }

		public LevelProgressFacade(
			IGameProgress progressService
		)
		{
			_progressService = progressService;

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