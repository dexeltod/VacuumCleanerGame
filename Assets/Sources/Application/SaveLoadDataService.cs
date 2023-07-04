using System.IO;
using Application.DI;
using Domain.Progress;
using Infrastructure.DTO;
using InfrastructureInterfaces;

namespace Application
{
	public class SaveLoadDataService : ISaveLoadDataService
	{
		private const string SavesDirectory = "/Saves/";

		private readonly BinaryDataSaveLoader _binaryDataSaveLoader;
		private readonly JsonDataSaveLoader _jsonDataLoader;
		private readonly IPersistentProgressService _persistentProgress;
		private GameProgressModel _gameProgress;

		private string _lastTime;
		private string _saveFilePath;

		public SaveLoadDataService()
		{
			string saveDirectoryPath = UnityEngine.Application.persistentDataPath + SavesDirectory;
			Directory.CreateDirectory(saveDirectoryPath);

			_jsonDataLoader = new JsonDataSaveLoader();
			_binaryDataSaveLoader = new BinaryDataSaveLoader();
			_persistentProgress = ServiceLocator.Container.GetSingle<IPersistentProgressService>();
		}

		public void SaveToJson(string fileName, object data) =>
			_jsonDataLoader.Save(fileName, data);

		public string LoadFromJson(string fileName) =>
			_jsonDataLoader.Load(fileName);

		public T LoadFromJson<T>(string fileName) =>
			_jsonDataLoader.Load<T>(fileName);

		public void SaveProgress() =>
			_binaryDataSaveLoader.Save(_persistentProgress.GameProgress);

		public GameProgressModel LoadProgress()
		{
			_gameProgress = _binaryDataSaveLoader.LoadProgress();
			return _gameProgress;
		}
	}
}