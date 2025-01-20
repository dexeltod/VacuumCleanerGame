using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Sources.BusinessLogic.Repository;
using Sources.BusinessLogic.Services;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using UnityEngine;
using VContainer;

namespace Sources.Boot.Scripts.Factories.Progress
{
	public class ProgressFactory
	{
		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
		private readonly IClearProgressFactory _clearProgressFactory;

		private readonly ISaveLoader _saveLoader;

		[Inject]
		public ProgressFactory(
			IProgressSaveLoadDataService progressSaveLoadDataService,
			IPersistentProgressService persistentProgressServiceProvider,
			ISaveLoader saveLoaderProvider,
			IClearProgressFactory clearProgressFactory,
			IProgressEntityRepository progressEntityRepositoryProvider,
			IPlayerModelRepository playerModelRepositoryProvider
		)
		{
			_progressSaveLoadDataService = progressSaveLoadDataService ??
			                               throw new ArgumentNullException(nameof(progressSaveLoadDataService));
			_saveLoader = saveLoaderProvider ?? throw new ArgumentNullException(nameof(saveLoaderProvider));
			_clearProgressFactory = clearProgressFactory ?? throw new ArgumentNullException(nameof(clearProgressFactory));
		}

		public async Task<IGlobalProgress> Create()
		{
			IGlobalProgress cloudSaves = await _progressSaveLoadDataService.LoadFromCloud();

			cloudSaves = await CreatNewIfNull(cloudSaves);

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

			loadedProgress = _clearProgressFactory.Create();

			await _saveLoader.Save(loadedProgress);
			return loadedProgress;
		}
	}
}