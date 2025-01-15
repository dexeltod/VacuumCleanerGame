using System;
using Sources.BuisenessLogic.ServicesInterfaces;
using Sources.DomainInterfaces;
using VContainer;

namespace Sources.Infrastructure.Services
{
	public class LevelProgressFacade : ILevelProgressFacade
	{
		private readonly IPersistentProgressServiceProvider _progressService;
		private const int OnePoint = 1;
		private const int MaxScoreDelta = 50;
		private const int MaxScore = 500;

		[Inject]
		public LevelProgressFacade(IPersistentProgressServiceProvider progressService) =>
			_progressService = progressService ?? throw new ArgumentNullException(nameof(progressService));

		public int CurrentLevel => _progressService.Self.GlobalProgress.LevelProgress.CurrentLevel;

		public int MaxTotalResourceCount =>
			_progressService.Self.GlobalProgress.ResourceModelReadOnly.MaxTotalResourceCount;

		private ILevelProgress LevelProgress => _progressService.Self.GlobalProgress.LevelProgress;

		private IResourceModel ResourceModel =>
			_progressService.Self.GlobalProgress.ResourceModelReadOnly as IResourceModel;

		private IResourceModelReadOnly ResourceModelReadOnly =>
			_progressService.Self.GlobalProgress.ResourceModelReadOnly as IResourceModelReadOnly;

		public void SetNextLevel()
		{
			LevelProgress.AddLevel(MaxScoreDelta, OnePoint);

			if (ResourceModelReadOnly.MaxTotalResourceCount >= MaxScore)
				return;

			ResourceModel.AddMaxTotalResourceModifier(MaxScoreDelta);
		}
	}
}