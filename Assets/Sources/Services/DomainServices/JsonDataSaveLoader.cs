using System.IO;
using UnityEngine;

namespace Sources.Services.DomainServices
{
	public class JsonDataSaveLoader
	{
		private readonly string _fileFormat;
		private string _directoryPath => Application.persistentDataPath + "/Data/";

		public JsonDataSaveLoader()
		{
			_fileFormat = ".json";
			Directory.CreateDirectory(_directoryPath);
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

		public T Load<T>(string file)
		{
			var json = GetJson(file);
			return JsonUtility.FromJson<T>(json);
		}

		public string Load(string file)
		{
			var json = GetJson(file);
			return JsonUtility.FromJson<string>(json);
		}

		private string GetJson(string fileName)
		{
			string path = _directoryPath + fileName + _fileFormat;

			string json = "";
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

		private void CreateNewFileIfNull(string filePath)
		{
			if (File.Exists(filePath) == false)
			{
				var file = File.Create(filePath);

				using (var writer = new StreamWriter(file))
				{
					writer.WriteLine("");
				}
			}
		}
	}
}