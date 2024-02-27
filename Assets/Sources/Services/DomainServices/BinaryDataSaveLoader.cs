using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Sources.Domain.Progress;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.Services.DomainServices
{
	public class BinaryDataSaveLoader : IBinaryDataSaveLoader
	{
		private const string SaveFileFormat = ".data";
		private const string SavesDirectory = "/Saves/";
		private const string SaveName = "save_";
		private const string TimeFormat = "ss.mm.hh.dd.MM.yyyy";
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

		private string DirectorySavePath => UnityEngine.Application.persistentDataPath + SavesDirectory;

		public void DeleteSaves()
		{
			string[] files = Directory.GetFiles(DirectorySavePath);

			if (files.Length == 0)
				return;

			foreach (string file in files)
				File.Delete(file);
		}

		public IGlobalProgress LoadProgress()
		{
			string[] files = Directory.GetFiles(DirectorySavePath);

			if (files.Length <= 0)
				throw new Exception("Directory does not have any files with the name " + DirectorySavePath);

			files = Directory.GetFiles(DirectorySavePath);

			string lastSaveFilePath = files.Last();

			using FileStream file = File.Open(lastSaveFilePath, FileMode.Open);
			{
				object loadedData = new BinaryFormatter().Deserialize(file);
				GlobalProgress globalProgress = (GlobalProgress)loadedData;
				return globalProgress;
			}
		}

		public void SetUniqueSaveFilePath()
		{
			_lastTime = DateTime.UtcNow.ToString(TimeFormat);
			_saveFilePath = DirectorySavePath + SaveName + _lastTime + SaveFileFormat;
		}
	}
}