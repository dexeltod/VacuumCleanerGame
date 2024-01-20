using System;
using System.IO;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Services.DomainServices.DTO;

namespace Sources.Services.DomainServices
{
	[Serializable] public class ProgressLoadDataService : IProgressLoadDataService
	{
		private const string SavesDirectory = "/Saves/";

		private readonly ISaveLoader _saveLoader;
		private readonly IPersistentProgressServiceConstructable _progressService;

		private readonly BinaryDataSaveLoader _binaryDataSaveLoader;
		private readonly JsonDataSaveLoader _jsonDataLoader;
		private readonly IPersistentProgressService _persistentProgress;
		private IGameProgressModel _gameProgress;

		public bool IsCallbackReceived { get; private set; }

		public event Func<IGameProgressModel> ProgressCleared;

		public ProgressLoadDataService(
			ISaveLoader saveLoader,
			IPersistentProgressServiceConstructable progressService,
			IPersistentProgressService persistentProgressService
		)
		{
			_saveLoader = saveLoader;
			_progressService = progressService;
			_binaryDataSaveLoader = new BinaryDataSaveLoader();

			string saveDirectoryPath = UnityEngine.Application.persistentDataPath + SavesDirectory;
			Directory.CreateDirectory(saveDirectoryPath);

			_jsonDataLoader = new JsonDataSaveLoader();
			_persistentProgress = persistentProgressService;
		}

		public async UniTask SaveToCloud(IGameProgressModel model, Action succeededCallback = null)
		{
			if (model != null)
				await SaveWithCallback(model);

			succeededCallback?.Invoke();
		}

		public async UniTask SaveToCloud(Action succeededCallback = null)
		{
			await SaveWithCallback(_persistentProgress.GameProgress);
			succeededCallback?.Invoke();
		}

		public async UniTask ClearSaves()
		{
			IGameProgressModel clearSave = ProgressCleared.Invoke();
			_progressService.Construct(clearSave);

			await SaveWithCallback(clearSave);
		}

		public async UniTask<IGameProgressModel> LoadFromCloud() =>
			await LoadWithCallback();

		public void SaveToJson(string fileName, object data) =>
			_jsonDataLoader.Save(fileName, data);

		public string LoadFromJson(string fileName) =>
			_jsonDataLoader.Load(fileName);

		public T LoadFromJson<T>(string fileName) =>
			_jsonDataLoader.Load<T>(fileName);

		public void SaveProgressBinary() =>
			_binaryDataSaveLoader.Save(_persistentProgress.GameProgress);

		public IGameProgressModel LoadProgressBinary()
		{
			_gameProgress = _binaryDataSaveLoader.LoadProgress();
			return _gameProgress;
		}

		private async UniTask SaveWithCallback(IGameProgressModel model)
		{
			IsCallbackReceived = false;
			await _saveLoader.Save(model, () => IsCallbackReceived = true);
		}

		private async UniTask<IGameProgressModel> LoadWithCallback()
		{
			IsCallbackReceived = false;
			return await _saveLoader.Load(() => IsCallbackReceived = true);
		}
	}
}