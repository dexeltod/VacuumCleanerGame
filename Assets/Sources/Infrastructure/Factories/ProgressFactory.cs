using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Providers;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Providers;
using Sources.InfrastructureInterfaces.Services;
using Sources.ServicesInterfaces;
using UnityEngine;
using VContainer;

namespace Sources.Infrastructure.Factories
{
	[Serializable] public class ProgressFactory : IProgressFactory
	{
		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
		private readonly IProgressCleaner _progressCleaner;
		private readonly ProgressServiceRegister _progressServiceRegister;

		private readonly ISaveLoaderProvider _saveLoaderProvider;

		[Inject]
		public ProgressFactory(
			IProgressSaveLoadDataService progressSaveLoadDataService,
			IPersistentProgressServiceProvider persistentProgressServiceProvider,
			ISaveLoaderProvider saveLoaderProvider,
			IProgressCleaner progressCleaner,
			IProgressEntityRepositoryProvider progressEntityRepositoryProvider,
			IAssetFactory assetFactory,
			IPlayerModelRepositoryProvider playerModelRepositoryProvider,
			ProgressServiceRegister progressServiceRegister
		)
		{
			_progressSaveLoadDataService = progressSaveLoadDataService ??
				throw new ArgumentNullException(nameof(progressSaveLoadDataService));
			_saveLoaderProvider = saveLoaderProvider ?? throw new ArgumentNullException(nameof(saveLoaderProvider));
			_progressCleaner = progressCleaner ?? throw new ArgumentNullException(nameof(progressCleaner));
			_progressServiceRegister = progressServiceRegister ??
				throw new ArgumentNullException(nameof(progressServiceRegister));
		}

		public async Task<IGlobalProgress> Create()
		{
			IGlobalProgress cloudSaves = await _progressSaveLoadDataService.LoadFromCloud();

			Debug.Log("Loaded from cloud");
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

			await _saveLoaderProvider.Self.Save(loadedProgress);
			return loadedProgress;
		}
	}
}