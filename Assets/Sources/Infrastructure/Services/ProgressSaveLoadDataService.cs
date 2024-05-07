using System;
using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Providers;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Providers;
using Sources.InfrastructureInterfaces.Services;
using Sources.Services.DomainServices;
using UnityEngine;
using VContainer;

namespace Sources.Infrastructure.Services
{
	[Serializable] public class ProgressSaveLoadDataService : IProgressSaveLoadDataService
	{
		private readonly ISaveLoaderProvider _saveLoader;
		private readonly IPersistentProgressServiceProvider _progressServiceProvider;

		private readonly BinaryDataSaveLoader _binaryDataSaveLoader;
		private readonly JsonDataSaveLoader _jsonDataLoader;
		private readonly IProgressCleaner _progressCleaner;

		private IGlobalProgress _globalProgress;

		[Inject]
		public ProgressSaveLoadDataService(
			ISaveLoaderProvider saveLoader,
			IPersistentProgressServiceProvider progressService,
			IInitialProgressFactory initialProgressFactory,
			IProgressCleaner progressCleaner
		)
		{
			_saveLoader = saveLoader ?? throw new ArgumentNullException(nameof(saveLoader));
			_progressServiceProvider = progressService ?? throw new ArgumentNullException(nameof(progressService));
			_progressCleaner = progressCleaner ?? throw new ArgumentNullException(nameof(progressCleaner));

			_jsonDataLoader = new JsonDataSaveLoader();
		}

		private ISaveLoader SaveLoaderImplementation => _saveLoader.Self;

		private IPersistentProgressService PersistentProgressService => _progressServiceProvider.Self;

		public async UniTask SaveToCloud(IGlobalProgress progress)
		{
			if (progress != null)
				await Save(progress);
		}

		public async UniTask SaveToCloud() =>
			await Save(PersistentProgressService.GlobalProgress);

		public async UniTask ClearSaves() =>
			await _saveLoader.Self.Save(_progressCleaner.CreateClearSaves());

		public async UniTask<IGlobalProgress> LoadFromCloud()
		{
			IGlobalProgress progress = await SaveLoaderImplementation.Load();

			if (progress != null && progress.IsAllProgressNotNull())
				return progress;

			Debug.LogError("GlobalProgress is null");
			return null;
		}

		public void SaveToJson(string fileName, object data) =>
			_jsonDataLoader.Save(fileName, data);

		public string LoadFromJson(string fileName) =>
			_jsonDataLoader.Load(fileName);

		public T LoadFromJson<T>(string fileName) =>
			_jsonDataLoader.Load<T>(fileName);

		private async UniTask Save(IGlobalProgress provider) =>
			await SaveLoaderImplementation.Save(provider);
	}
}