using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Cysharp.Threading.Tasks;
using Model.Infrastructure.Data;
using UnityEngine;

namespace ViewModel.Infrastructure.Services
{
	public class BinaryDataSaveLoader
	{
		private const string SaveFileFormat = ".data";
		private const string SavesDirectory = "/Saves/";
		private const string SaveName = "save_";
		private const string TimeFormat = "ss.mm.hh.dd.MM.yyyy";

		private string _directorySavePath => Application.persistentDataPath + SavesDirectory;
		private string _lastTime;
		private string _saveFilePath;

		public void Save(object data)
		{
			DeleteSaves();
			SetUniqueSaveFilePath();
			
			using (FileStream saveFile = File.Create(_saveFilePath))
			{
				new BinaryFormatter().Serialize(saveFile, data);
			}
		}

		private void DeleteSaves()
		{
			File.Delete(_saveFilePath);
		}

		public async UniTask<GameProgressModel> LoadProgress()
		{
			string[] files = Directory.GetFiles(_directorySavePath);

			if (files.Length <= 0)
				await CreateNewProgressByBinary();

			files = Directory.GetFiles(_directorySavePath);

			string lastSaveFilePath = files.Last();

			using FileStream file = File.Open(lastSaveFilePath, FileMode.Open);
			{
				object loadedData = new BinaryFormatter().Deserialize(file);
				GameProgressModel gameProgress = (GameProgressModel)loadedData;
				return gameProgress;
			}
		}

		public async UniTask CreateNewProgressByBinary()
		{
			SetUniqueSaveFilePath();
			GameProgressModel gameProgress = new();

			using (FileStream saveFile = File.Create(_saveFilePath))
			{
				new BinaryFormatter().Serialize(saveFile, gameProgress);
			}
			
			Debug.Log("new progress created");
		}

		private void SetUniqueSaveFilePath()
		{
			_lastTime = DateTime.UtcNow.ToString(TimeFormat);
			_saveFilePath = _directorySavePath + SaveName + _lastTime + SaveFileFormat;
		}
	}
}