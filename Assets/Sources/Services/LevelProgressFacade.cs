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
		private readonly IResourceModelModifiable _resourceModelModifiable;

		[Inject]
		public LevelProgressFacade(
			IPersistentProgressServiceProvider progressService
		)
		{
			if (progressService == null) throw new ArgumentNullException(nameof(progressService));
			_progressService = progressService.Self.GlobalProgress.LevelProgress;
			_resourceModelModifiable
				= progressService.Self.GlobalProgress.ResourceModelReadOnly as IResourceModelModifiable;
		}

		public int CurrentLevel => _progressService.CurrentLevel;
		public int MaxTotalResourceCount => _progressService.MaxTotalResourceCount;

		public void SetNextLevel()
		{
			_progressService.AddLevel(MaxScoreDelta, OnePoint);
			_resourceModelModifiable.AddMaxTotalResourceModifier(MaxScoreDelta);
		}
	}
}