using UnityEngine;

namespace Model.Infrastructure.Data
{
	public class ProgressNames
	{
		public readonly string SaveFileFormat = ".data";
		public string SavesDirectory => Application.persistentDataPath + "/Saves/";
		public readonly string SaveName = "save_";
	}
}