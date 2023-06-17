using System;
using System.Collections.Generic;

namespace Model.Data
{
	[Serializable]
	public class ShopProgress
	{
		public readonly int MaxPointCount = 6;

		private List<string> _progressNames;
		private List<int> _progressPointValues;

		public ShopProgress(List<int> progressPointValues, List<string> progressNames)
		{
			_progressNames = progressNames;
			_progressPointValues = progressPointValues;
		}

		public List<Tuple<string, int>> GetAllProgress()
		{
			List<Tuple<string, int>> progress = new();

			for (int i = 0; i < _progressNames.Count; i++)
				progress.Add(new Tuple<string, int>(_progressNames[i], _progressPointValues[i]));

			return progress;
		}

		public Tuple<string, int> GetProgressByName(string name)
		{
			if (_progressNames == null || _progressNames.Count == 0)
				throw new NullReferenceException("Progress is not initialized");
			
			for (int i = 0; i < _progressNames.Count; i++)
			{
				if (_progressNames[i] == name)
					return new Tuple<string, int>(_progressNames[i], _progressPointValues[i]);
			}

			throw new InvalidOperationException("Invalid progress name: " + name);
		}

		public void ChangeProgressValue(string progressName, int progressValue)
		{
			if (_progressNames.Contains(progressName) == false)
				return;

			int nameIndex = _progressNames.FindIndex(x => x == progressName);

			if (nameIndex >= 0)
				_progressPointValues[nameIndex] = progressValue;
		}
	}
}