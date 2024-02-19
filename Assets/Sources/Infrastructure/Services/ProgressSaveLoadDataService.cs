using System;
using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Factories.Domain;
using Sources.Infrastructure.Providers;
using Sources.InfrastructureInterfaces.Providers;
using Sources.Services.DomainServices.DTO;
using VContainer;

namespace Sources.Services.DomainServices
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

		private ISaveLoader SaveLoaderImplementation => _saveLoader.Implementation;

		public bool IsCallbackReceived { get; private set; }

		private IPersistentProgressService PersistentProgressService => _progressServiceProvider.Implementation;

		public async UniTask SaveToCloud(IGlobalProgress progress, Action succeededCallback = null)
		{
			if (progress != null)
				await Save(progress);

			succeededCallback?.Invoke();
		}

		public async UniTask SaveToCloud(Action succeededCallback = null)
		{
			await Save(PersistentProgressService.GlobalProgress);
			succeededCallback?.Invoke();
		}

		public async UniTask ClearSaves() =>
			await _progressCleaner.ClearAndSaveCloud();

		public async UniTask<IGlobalProgress> LoadFromCloud()
		{
			IsCallbackReceived = false;
			return await SaveLoaderImplementation.Load(() => IsCallbackReceived = true);
		}

		public void SaveToJson(string fileName, object data) =>
			_jsonDataLoader.Save(fileName, data);

		public string LoadFromJson(string fileName) =>
			_jsonDataLoader.Load(fileName);

		public T LoadFromJson<T>(string fileName) =>
			_jsonDataLoader.Load<T>(fileName);

		private async UniTask Save(IGlobalProgress provider)
		{
			IsCallbackReceived = false;
			await SaveLoaderImplementation.Save(provider, () => IsCallbackReceived = true);
		}
	}
}