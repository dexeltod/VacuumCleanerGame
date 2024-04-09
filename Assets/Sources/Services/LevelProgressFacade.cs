using System;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces.Providers;
using Sources.ServicesInterfaces;
using VContainer;

namespace Sources.Services
{
	public class LevelProgressFacade : ILevelProgressFacade
	{
		private const int OnePoint = 1;
		private const int MaxScoreDelta = 50;

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
			_progressService.AddLevel(MaxScoreDelta, OnePoint);
	}
}