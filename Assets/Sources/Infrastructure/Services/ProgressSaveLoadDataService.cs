using System;
using Cysharp.Threading.Tasks;
using Sources.BusinessLogic.Interfaces.Factory;
using Sources.BusinessLogic.Services;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Services.DomainServices;
using VContainer;

namespace Sources.Infrastructure.Services
{
	[Serializable]
	public class ProgressSaveLoadDataService : IProgressSaveLoadDataService
	{
		private readonly BinaryDataSaveLoader _binaryDataSaveLoader;
		private readonly JsonDataSaveLoader _jsonDataLoader;
		private readonly IProgressCleaner _progressCleaner;
		private readonly IPersistentProgressService _progressService;
		private readonly ISaveLoader _saveLoaderImplementation;

		private IGlobalProgress _globalProgress;

		[Inject]
		public ProgressSaveLoadDataService(
			ISaveLoader saveLoader,
			IPersistentProgressService progressService,
			IInitialProgressFactory initialProgressFactory,
			IProgressCleaner progressCleaner
		)
		{
			_saveLoaderImplementation = saveLoader ?? throw new ArgumentNullException(nameof(saveLoader));
			_progressService = progressService ?? throw new ArgumentNullException(nameof(progressService));
			_progressCleaner = progressCleaner ?? throw new ArgumentNullException(nameof(progressCleaner));

			_jsonDataLoader = new JsonDataSaveLoader();
		}

		public bool IsCallbackReceived { get; private set; }

		public async UniTask SaveToCloud(Action succeededCallback = null)
		{
			await Save(_progressService.GlobalProgress);
			succeededCallback?.Invoke();
		}

		public async UniTask ClearSaves() => await _saveLoaderImplementation.Save(_progressCleaner.CreateClear());

		public async UniTask<IGlobalProgress> LoadFromCloud() => await _saveLoaderImplementation.Load();

		public void SaveToJson(string fileName, object data) => _jsonDataLoader.Save(fileName, data);

		public string LoadFromJson(string fileName) => _jsonDataLoader.Load(fileName);

		public T LoadFromJson<T>(string fileName) => _jsonDataLoader.Load<T>(fileName);

		private async UniTask Save(IGlobalProgress provider)
		{
			IsCallbackReceived = false;
			await _saveLoaderImplementation.Save(provider, () => IsCallbackReceived = true);
		}
	}
}
