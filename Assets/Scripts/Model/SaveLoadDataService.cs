using System.IO;
using Cysharp.Threading.Tasks;
using Model.DI;
using Model.Infrastructure;
using Model.Infrastructure.Data;
using Model.Infrastructure.Services;
using Model.Infrastructure.Services.Factories;
using UnityEngine;

namespace Model
{
	public class SaveLoadDataService : ISaveLoadDataService
	{
		private const string SavesDirectory = "/Saves/";

		private readonly BinaryDataSaveLoader _binaryDataSaveLoader;
		private readonly JsonDataSaveLoader _jsonDataLoader;
		private GameProgress _gameProgress;

		private string _lastTime;
		private string _saveFilePath;

		public SaveLoadDataService(GameProgressFactory gameProgressFactory)
		{
			string saveDirectoryPath = Application.persistentDataPath + SavesDirectory;
			Directory.CreateDirectory(saveDirectoryPath);
			
			_jsonDataLoader = new JsonDataSaveLoader();
			_binaryDataSaveLoader = new BinaryDataSaveLoader(gameProgressFactory);
			
			_gameProgress = ServiceLocator.Container.GetSingle<IPersistentProgressService>().GameProgress;
		}

		public void SaveToJson(string fileName, object data) => 
			_jsonDataLoader.Save(fileName, data);

		public string LoadFromJson(string fileName) => 
			_jsonDataLoader.Load(fileName);

		public T LoadFromJson<T>(string fileName) 
		{
			return _jsonDataLoader.Load<T>(fileName);
		}
		
		public void SaveProgress() =>
			_binaryDataSaveLoader.Save(_gameProgress);

		public async void SetStartProgress() => 
			_gameProgress = await _binaryDataSaveLoader.CreateNewProgressByBinary();

		public async UniTask<GameProgress> LoadProgress() =>
			_gameProgress = await _binaryDataSaveLoader.LoadProgress();
	}
}