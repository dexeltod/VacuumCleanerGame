using System;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces.Providers;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs.Scripts;
using Sources.Utils.ConstantNames;
using VContainer;

namespace Sources.Services
{
	public class LevelProgressFacade : ILevelProgressFacade
	{
		private const int OnePoint = 1;
		private const int MaxScoreDelta = 100;

		private readonly ILevelProgress _progressService;

		[Inject]
		public LevelProgressFacade(
			IPersistentProgressServiceProvider progressService
		)
		{
			if (progressService == null) throw new ArgumentNullException(nameof(progressService));
			_progressService = progressService.Implementation.GlobalProgress.LevelProgress;
		}

		public int CurrentLevel => _progressService.CurrentLevel;
		public int MaxScoreCount => _progressService.MaxScoreCount;

		public void SetNextLevel() =>
			_progressService.AddLevel(OnePoint, MaxScoreDelta);
	}

	public class LevelDifficultyMagnifier : ILevelDifficultyMagnifier
	{
		private readonly ProgressionConfig _progressionConfig;

		public LevelDifficultyMagnifier(ProgressionConfig progressionConfig)
		{
			_progressionConfig = progressionConfig ?? throw new ArgumentNullException(nameof(progressionConfig));
		}

		public void IncreaseMaxSand() { }
	}

	public interface ILevelDifficultyMagnifier { }
}