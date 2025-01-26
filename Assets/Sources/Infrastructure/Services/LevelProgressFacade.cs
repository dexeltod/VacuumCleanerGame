using System;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.DomainInterfaces;
using VContainer;

namespace Sources.Infrastructure.Services
{
	public class LevelProgressFacade : ILevelProgressFacade
	{
		private const int OnePoint = 1;
		private const int MaxScoreDelta = 50;
		private const int MaxScore = 500;
		private readonly IPersistentProgressService _progressService;

		[Inject]
		public LevelProgressFacade(IPersistentProgressService progressService) =>
			_progressService = progressService ?? throw new ArgumentNullException(nameof(progressService));

		private ILevelProgress LevelProgress => _progressService.GlobalProgress.LevelProgress;

		private IResourceModel ResourceModel => _progressService.GlobalProgress.ResourceModel;

		private IResourceModelReadOnly ResourceModelReadOnly => _progressService.GlobalProgress.ResourceModel;

		public int CurrentLevel => _progressService.GlobalProgress.LevelProgress.CurrentLevel;

		public int MaxTotalResourceCount => _progressService.GlobalProgress.ResourceModel.MaxTotalResourceCount;

		public void SetNextLevel()
		{
			LevelProgress.AddLevel(MaxScoreDelta, OnePoint);

			if (ResourceModelReadOnly.MaxTotalResourceCount >= MaxScore)
				return;

			ResourceModel.AddMaxTotalResourceModifier(MaxScoreDelta);
		}
	}
}