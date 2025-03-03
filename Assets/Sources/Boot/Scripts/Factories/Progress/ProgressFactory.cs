using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Sources.BusinessLogic.Interfaces.Factory;
using Sources.BusinessLogic.Services;
using Sources.DomainInterfaces;
using VContainer;

namespace Sources.Boot.Scripts.Factories.Progress
{
	public class ProgressFactory : IProgressFactory
	{
		private readonly IProgressCleaner _progressCleaner;
		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;

		[Inject]
		public ProgressFactory(
			IProgressSaveLoadDataService progressSaveLoadDataService,
			IProgressCleaner progressCleaner
		)
		{
			_progressSaveLoadDataService = progressSaveLoadDataService
			                               ?? throw new ArgumentNullException(nameof(progressSaveLoadDataService));
			_progressCleaner = progressCleaner ?? throw new ArgumentNullException(nameof(progressCleaner));
		}

		public async Task<IGlobalProgress> Create()
		{
			IGlobalProgress cloudSaves = await _progressSaveLoadDataService.LoadFromCloud();

			cloudSaves = await CreatNewIfNull(cloudSaves);

			cloudSaves.ResourceModel.SetMaxTotalResource(cloudSaves.LevelProgress.MaxTotalResourceCount);

			return cloudSaves;
		}

		private async UniTask<IGlobalProgress> CreatNewIfNull(IGlobalProgress loadedProgress)
		{
			if (loadedProgress != null && loadedProgress.Validate())
				return loadedProgress;

			loadedProgress = _progressCleaner.CreateClear();

			await _progressSaveLoadDataService.SaveToCloud();

			return loadedProgress;
		}
	}
}
