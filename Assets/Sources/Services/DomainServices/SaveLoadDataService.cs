using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Sources.DIService;
using Sources.DomainInterfaces;
using Sources.Services.DomainServices.DTO;
using Sources.ServicesInterfaces;
using Unity.Services.CloudSave;
using UnityEngine;

namespace Sources.Services.DomainServices
{
	[Serializable]
	public class SaveLoadDataService : ISaveLoadDataService
	{
		private const string SavesDirectory = "/Saves/";
		private const string GameProgressKey = "GameProgress";

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

		public void SaveToUnityCloud()
		{
			string a = JsonConvert.SerializeObject(_persistentProgress.GameProgress);

			CloudSaveService.Instance.Data.ForceSaveAsync(new Dictionary<string, object>
				{
					{ GameProgressKey, _persistentProgress.GameProgress }
				}
			);
		}

		public async UniTask<IGameProgressModel> LoadFromUnityCloud()
		{
			Dictionary<string, string> keyAndJsonSaves = await CloudSaveService.Instance.Data.LoadAsync
			(
				new HashSet<string> { GameProgressKey }
			);

			// return JsonConvert.DeserializeObject<IGameProgressModel>(keyAndJsonSaves.Values.LastOrDefault());
			return null;
		}

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
	}
}