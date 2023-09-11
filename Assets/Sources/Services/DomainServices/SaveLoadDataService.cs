using System;
using System.IO;
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

namespace Sources.Services.DomainServices
{
	[Serializable]
	public class SaveLoadDataService : ISaveLoadDataService
	{
		private const string SavesDirectory = "/Saves/";

		private readonly BinaryDataSaveLoader _binaryDataSaveLoader;
		private readonly JsonDataSaveLoader _jsonDataLoader;
		private readonly IPersistentProgressService _persistentProgress;
		private IGameProgressModel _gameProgress;

		public SaveLoadDataService()
		{
			_binaryDataSaveLoader = new BinaryDataSaveLoader();

			string saveDirectoryPath = UnityEngine.Application.persistentDataPath + SavesDirectory;
			Directory.CreateDirectory(saveDirectoryPath);

			_jsonDataLoader = new JsonDataSaveLoader();
			_persistentProgress = GameServices.Container.Get<IPersistentProgressService>();
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