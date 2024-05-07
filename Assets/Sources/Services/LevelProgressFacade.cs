using System;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces.Providers;
using Sources.ServicesInterfaces;
using VContainer;

namespace Sources.Services
{
	public class LevelProgressFacade : ILevelProgressFacade
	{
		private readonly IPersistentProgressServiceProvider _progressService;
		private const int OnePoint = 1;
		private const int MaxScoreDelta = 50;

		[Inject]
		public LevelProgressFacade(IPersistentProgressServiceProvider progressService) =>
			_progressService = progressService ?? throw new ArgumentNullException(nameof(progressService));

		public int CurrentLevel => _progressService.Self.GlobalProgress.LevelProgress.CurrentLevel;

		public int MaxTotalResourceCount =>
			_progressService.Self.GlobalProgress.ResourceModelReadOnly.MaxTotalResourceCount;

		private IResourceModel ResourceModel =>
			_progressService.Self.GlobalProgress.ResourceModelReadOnly as IResourceModel;

		public void SetNextLevel()
		{
			_progressService.Self.GlobalProgress.LevelProgress.AddLevel(MaxScoreDelta, OnePoint);
			ResourceModel.AddMaxTotalResourceModifier(MaxScoreDelta);
		}
	}
}