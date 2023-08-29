using System.IO;
using Sources.DIService;
using Sources.Domain.Progress;
using Sources.DomainInterfaces;
using Sources.Services.DomainServices.DTO;
using Sources.ServicesInterfaces;

namespace Sources.Services.DomainServices
{
	public class SaveLoadDataService : ISaveLoadDataService
	{
		private const string SavesDirectory = "/Saves/";

		private readonly BinaryDataSaveLoader _binaryDataSaveLoader;
		private readonly JsonDataSaveLoader _jsonDataLoader;
		private readonly IPersistentProgressService _persistentProgress;
		private IGameProgressModel _gameProgress;

		private string _lastTime;
		private string _saveFilePath;

		public SaveLoadDataService()
		{
			string saveDirectoryPath = UnityEngine.Application.persistentDataPath + SavesDirectory;
			Directory.CreateDirectory(saveDirectoryPath);

			_jsonDataLoader = new JsonDataSaveLoader();
			_binaryDataSaveLoader = new BinaryDataSaveLoader();
			_persistentProgress = GameServices.Container.Get<IPersistentProgressService>();
		}

		public void SaveToJson(string fileName, object data) =>
			_jsonDataLoader.Save(fileName, data);

		public string LoadFromJson(string fileName) =>
			_jsonDataLoader.Load(fileName);

		public T LoadFromJson<T>(string fileName) =>
			_jsonDataLoader.Load<T>(fileName);

		public void SaveProgress() =>
			_binaryDataSaveLoader.Save(_persistentProgress.GameProgress);

		public IGameProgressModel LoadProgress()
		{
			_gameProgress = _binaryDataSaveLoader.LoadProgress();
			return _gameProgress;
		}
	}
}