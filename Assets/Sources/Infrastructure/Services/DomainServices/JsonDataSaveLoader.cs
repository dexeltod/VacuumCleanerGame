using System.IO;
using UnityEngine;

namespace Sources.Infrastructure.Services.DomainServices
{
	public class JsonDataSaveLoader
	{
		private readonly string _fileFormat;

		public JsonDataSaveLoader()
		{
			_fileFormat = ".json";
			Directory.CreateDirectory(_directoryPath);
		}

		private string _directoryPath => Application.persistentDataPath + "/Data/";

		public T Load<T>(string file)
		{
			string json = GetJson(file);
			return JsonUtility.FromJson<T>(json);
		}

		public string Load(string file)
		{
			string json = GetJson(file);
			return JsonUtility.FromJson<string>(json);
		}

		public void Save(string fileName, object data)
		{
			string file = JsonUtility.ToJson(data);
			string path = _directoryPath + fileName + _fileFormat;

			using (var writer = new StreamWriter(path))
			{
				writer.WriteLine(file);
			}
		}

		private void CreateNewFileIfNull(string filePath)
		{
			if (File.Exists(filePath) == false)
			{
				FileStream file = File.Create(filePath);

				using (var writer = new StreamWriter(file))
				{
					writer.WriteLine("");
				}
			}
		}

		private string GetJson(string fileName)
		{
			string path = _directoryPath + fileName + _fileFormat;

			var json = "";
			json = ReadFile(path, json);
			return json;
		}

		private string ReadFile(string filePath, string json)
		{
			CreateNewFileIfNull(filePath);

			using (var reader = new StreamReader(filePath))
			{
				string line;

				while ((line = reader.ReadLine()) != null)
					json += line;
			}

			return json;
		}
	}
}