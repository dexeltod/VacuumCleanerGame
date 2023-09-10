using System;
using System.IO;
using Agava.YandexGames;
using Newtonsoft.Json;
using Sources.Application;
using Sources.DIService;
using Sources.Domain.Progress;
using Sources.DomainInterfaces;
using Sources.Services.DomainServices.DTO;
using UnityEngine;
#if !YANDEX_GAMES
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Linq;
#endif

#if !YANDEX_GAMES
using Unity.Services.CloudSave;
#endif

namespace Sources.Services.DomainServices
{
	[Serializable]
	public class SaveLoadDataService : ISaveLoadDataService
	{
		private const string SavesDirectory = "/Saves/";
		private const string GameProgressKey = "GameProgress";

#if !YANDEX_GAMES
		private readonly BinaryDataSaveLoader _binaryDataSaveLoader;
#endif
		private readonly JsonDataSaveLoader _jsonDataLoader;
		private readonly IPersistentProgressService _persistentProgress;
		private readonly JsonSerializerSettings _jsonSerializerSettings;
		private IGameProgressModel _gameProgress;

		public SaveLoadDataService(ICoroutineRunner coroutineRunner)
		{
#if !YANDEX_GAMES
			_binaryDataSaveLoader = new BinaryDataSaveLoader();
#endif
			string saveDirectoryPath = UnityEngine.Application.persistentDataPath + SavesDirectory;
			Directory.CreateDirectory(saveDirectoryPath);

			_jsonDataLoader = new JsonDataSaveLoader();
			_persistentProgress = GameServices.Container.Get<IPersistentProgressService>();

			_jsonSerializerSettings = new JsonSerializerSettings
				{ ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor };
		}

		public void SaveToUnityCloud()
		{
			GameProgressModel model = _persistentProgress.GameProgress as GameProgressModel;

			string dataJsonUtility = JsonUtility.ToJson(model);
#if !YANDEX_GAMES
			CloudSaveService.Instance.Data.ForceSaveAsync(new Dictionary<string, object>
				{
					{ GameProgressKey, dataJsonUtility }
				}
			);
#endif
		}

#if !YANDEX_GAMES
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
#endif

#if YANDEX_GAMES && !UNITY_EDITOR
		public IGameProgressModel LoadFromYandex()
		{
			if (PlayerAccount.IsAuthorized)
				PlayerAccount.GetCloudSaveData(OnYandexCloudSaveLoaded, OnYandexLoadError);

			return _gameProgress;
		}

		private void OnYandexLoadError(string obj) =>
			throw new Exception(obj);

		private void OnYandexCloudSaveLoaded(string json) =>
			_gameProgress = JsonUtility.FromJson<GameProgressModel>(json);

		public void SaveToYandex(string jsonSave)
		{
			PlayerAccount.SetCloudSaveData(jsonSave);
		}
#endif
		public void SaveToJson(string fileName, object data) =>
			_jsonDataLoader.Save(fileName, data);

		public string LoadFromJson(string fileName) =>
			_jsonDataLoader.Load(fileName);

		public T LoadFromJson<T>(string fileName) =>
			_jsonDataLoader.Load<T>(fileName);

#if !YANDEX_GAMES
		public void SaveProgressBinary() =>
			_binaryDataSaveLoader.Save(_persistentProgress.GameProgress);

		public IGameProgressModel LoadProgressBinary()
		{
			_gameProgress = _binaryDataSaveLoader.LoadProgress();
			return _gameProgress;
		}
#endif

		private IGameProgressModel DeserializeJson(string jsonSave)
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
	}
}