using System;
using System.IO;
using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Services.DomainServices.DTO;

namespace Sources.Services.DomainServices
{
	[Serializable] public class ProgressSaveLoadDataService : IProgressSaveLoadDataService
	{
	

		private readonly ISaveLoader _saveLoader;
		private readonly IPersistentProgressServiceConstructable _progressService;

		private readonly BinaryDataSaveLoader _binaryDataSaveLoader;
		private readonly JsonDataSaveLoader _jsonDataLoader;
		private readonly IPersistentProgressService _persistentProgress;

		private IGameProgressProvider _gameProgress;

		public bool IsCallbackReceived { get; private set; }

		public event Func<IGameProgressProvider> ProgressCleared;

		public ProgressSaveLoadDataService(
			ISaveLoader saveLoader,
			IPersistentProgressServiceConstructable progressService,
			IPersistentProgressService persistentProgressService
		)
		{
			_saveLoader = saveLoader ?? throw new ArgumentNullException(nameof(saveLoader));
			_progressService = progressService ?? throw new ArgumentNullException(nameof(progressService));
			_persistentProgress = persistentProgressService ??
				throw new ArgumentNullException(nameof(persistentProgressService));

			_jsonDataLoader = new JsonDataSaveLoader();
		}

		public async UniTask SaveToCloud(IGameProgressProvider provider, Action succeededCallback = null)
		{
			if (provider != null)
				await Save(provider);

			succeededCallback?.Invoke();
		}

		public async UniTask SaveToCloud(Action succeededCallback = null)
		{
			await Save(_persistentProgress.GameProgress);
			succeededCallback?.Invoke();
		}

		public async UniTask ClearSaves()
		{
			IGameProgressProvider clearSave = ProgressCleared.Invoke();
			_progressService.Set(clearSave);

			await Save(clearSave);
		}

		public async UniTask<IGameProgressProvider> LoadFromCloud()
		{
			IsCallbackReceived = false;
			return await _saveLoader.Load(() => IsCallbackReceived = true);
		}

		public void SaveToJson(string fileName, object data) =>
			_jsonDataLoader.Save(fileName, data);

		public string LoadFromJson(string fileName) =>
			_jsonDataLoader.Load(fileName);

		public T LoadFromJson<T>(string fileName) =>
			_jsonDataLoader.Load<T>(fileName);

		private async UniTask Save(IGameProgressProvider provider)
		{
			IsCallbackReceived = false;
			await _saveLoader.Save(provider, () => IsCallbackReceived = true);
		}
	}
}