using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Model
{
	public class BinaryDataSaveLoader
	{
		private const string SaveFileFormat = ".data";
		private const string SavesDirectory = "/Saves/";
		private const string SaveName = "save_";

		private string _lastTime;
		private string _saveFilePath;
		private string _saveDirectoryPath => Application.persistentDataPath + SavesDirectory;
		private readonly GameProgressFactory _gameProgressFactory;

		public BinaryDataSaveLoader(GameProgressFactory gameProgressFactory)
		{
			_gameProgressFactory = gameProgressFactory;
		}

		public void Save(object data)
		{
			using (FileStream saveFile = File.Create(_saveFilePath))
			{
				new BinaryFormatter().Serialize(saveFile, data);
			}
		}

		public async UniTask<GameProgress> LoadProgress()
		{
			string[] files = Directory.GetFiles(_saveDirectoryPath);

			if (files.Length <= 0)
				await CreateNewProgressByBinary();

			files = Directory.GetFiles(_saveDirectoryPath);

			string lastSaveFilePath = files.Last();

			using FileStream file = File.Open(lastSaveFilePath, FileMode.Open);
			{
				object loadedData = new BinaryFormatter().Deserialize(file);
				GameProgress gameProgress = (GameProgress)loadedData;
				return gameProgress;
			}
		}

		public async Task<GameProgress> CreateNewProgressByBinary()
		{
			SetUniqueSaveFilePath();
			GameProgress gameProgress = await _gameProgressFactory.CreateProgress();

			using (FileStream saveFile = File.Create(_saveFilePath))
			{
				new BinaryFormatter().Serialize(saveFile, gameProgress);
			}

			return gameProgress;
		}

		private void SetUniqueSaveFilePath()
		{
			_lastTime = DateTime.UtcNow.ToString("ss.mm.hh.dd.MM.yyyy");
			_saveFilePath = _saveDirectoryPath + SaveName + _lastTime + SaveFileFormat;
		}
	}
}