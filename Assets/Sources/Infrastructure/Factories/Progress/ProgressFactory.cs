using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Sources.BuisenessLogic.Repository;
using Sources.BuisenessLogic.Services;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using UnityEngine;
using VContainer;

namespace Sources.Infrastructure.Factories.Progress
{
	public class ProgressFactory
	{
		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
		private readonly IProgressCleaner _progressCleaner;
		private readonly ProgressServiceRegister _progressServiceRegister;

		private readonly ISaveLoader _saveLoader;

		[Inject]
		public ProgressFactory(
			IProgressSaveLoadDataService progressSaveLoadDataService,
			IPersistentProgressService persistentProgressServiceProvider,
			ISaveLoader saveLoaderProvider,
			IProgressCleaner progressCleaner,
			IProgressEntityRepository progressEntityRepositoryProvider,
			IPlayerModelRepository playerModelRepositoryProvider,
			ProgressServiceRegister progressServiceRegister
		)
		{
			_progressSaveLoadDataService = progressSaveLoadDataService ??
			                               throw new ArgumentNullException(nameof(progressSaveLoadDataService));
			_saveLoader = saveLoaderProvider ?? throw new ArgumentNullException(nameof(saveLoaderProvider));
			_progressCleaner = progressCleaner ?? throw new ArgumentNullException(nameof(progressCleaner));
			_progressServiceRegister = progressServiceRegister ??
			                           throw new ArgumentNullException(nameof(progressServiceRegister));
		}

		public async Task<IGlobalProgress> Create()
		{
			IGlobalProgress cloudSaves = await _progressSaveLoadDataService.LoadFromCloud();

			cloudSaves = await CreatNewIfNull(cloudSaves);

			_progressServiceRegister.Do(cloudSaves);

			return cloudSaves;
		}

		private async UniTask<IGlobalProgress> CreatNewIfNull(IGlobalProgress loadedProgress)
		{
			Debug.Log($"Loaded progress is {loadedProgress}");

			if (loadedProgress != null && loadedProgress.Validate())
			{
				Debug.Log("Loaded progress is valid");
				return loadedProgress;
			}

			Debug.Log("New progress creating");

			loadedProgress = _progressCleaner.Clear();

			await _saveLoader.Save(loadedProgress);
			return loadedProgress;
		}
	}
}