using System.IO;
using Cysharp.Threading.Tasks;
using Model.Data;
using Model.DI;
using UnityEngine;
using ViewModel.Infrastructure;
using ViewModel.Infrastructure.Services;

namespace Model
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
			string saveDirectoryPath = Application.persistentDataPath + SavesDirectory;
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