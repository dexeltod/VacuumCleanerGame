using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sources.DIService;
using Sources.Domain.Progress;
using Sources.DomainInterfaces;
using Sources.Services.DomainServices.DTO;
using Unity.Plastic.Newtonsoft.Json;
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
		private readonly JsonSerializerSettings _jsonSerializerSettings;
		private IGameProgressModel _gameProgress;

		public SaveLoadDataService()
		{
			string saveDirectoryPath = Application.persistentDataPath + SavesDirectory;
			Directory.CreateDirectory(saveDirectoryPath);

			_jsonDataLoader = new JsonDataSaveLoader();
			_binaryDataSaveLoader = new BinaryDataSaveLoader();
			_persistentProgress = GameServices.Container.Get<IPersistentProgressService>();

			_jsonSerializerSettings = new JsonSerializerSettings();
			_jsonSerializerSettings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
		}

		public void SaveToUnityCloud()
		{
			GameProgressModel model = _persistentProgress.GameProgress as GameProgressModel;

			string dataJson = JsonConvert.SerializeObject(model);
			string dataJsonUtility = JsonUtility.ToJson(model);

			CloudSaveService.Instance.Data.ForceSaveAsync(new Dictionary<string, object>
				{
					{ GameProgressKey, dataJsonUtility }
				}
			);
		}

		public async UniTask<IGameProgressModel> LoadFromUnityCloud()
		{
			Dictionary<string, string> keyAndJsonSaves = await CloudSaveService
				.Instance
				.Data
				.LoadAsync
				(
					new HashSet<string> { GameProgressKey }
				);

			string jsonSave = keyAndJsonSaves.Values.LastOrDefault();

			return TryDeserialize(jsonSave);
		}

		private IGameProgressModel TryDeserialize(string jsonSave)
		{
			try
			{
				GameProgressModel model = JsonUtility.FromJson<GameProgressModel>(jsonSave);

				return model;
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				Debug.Log("New progress will be created");
				return null;
			}
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