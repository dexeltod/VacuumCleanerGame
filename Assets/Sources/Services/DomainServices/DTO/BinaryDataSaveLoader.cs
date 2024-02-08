using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Sources.Domain.Progress;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.Services.DomainServices.DTO
{
	public class BinaryDataSaveLoader : IDataSaveLoader
	{
		private const string SaveFileFormat = ".data";
		private const string SavesDirectory = "/Saves/";
		private const string SaveName = "save_";
		private const string TimeFormat = "ss.mm.hh.dd.MM.yyyy";

		private string _directorySavePath => UnityEngine.Application.persistentDataPath + SavesDirectory;
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

		public void DeleteSaves()
		{
			string[] files = Directory.GetFiles(_directorySavePath);

			if (files.Length == 0)
				return;

			foreach (string file in files)
				File.Delete(file);
		}

		public IGameProgressProvider LoadProgress()
		{
			string[] files = Directory.GetFiles(_directorySavePath);

			if (files.Length <= 0)
				throw new Exception("Directory does not have any files with the name " + _directorySavePath);

			files = Directory.GetFiles(_directorySavePath);

			string lastSaveFilePath = files.Last();

			using FileStream file = File.Open(lastSaveFilePath, FileMode.Open);
			{
				object loadedData = new BinaryFormatter().Deserialize(file);
				GameProgressProvider gameProgress = (GameProgressProvider)loadedData;
				return gameProgress;
			}
		}

		public void SetUniqueSaveFilePath()
		{
			_lastTime = DateTime.UtcNow.ToString(TimeFormat);
			_saveFilePath = _directorySavePath + SaveName + _lastTime + SaveFileFormat;
		}
	}
}