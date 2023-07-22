using System;
using System.Collections.Generic;

namespace Sources.Core.Domain.Progress
{
	[Serializable]
	public class Progress
	{
		public readonly int MaxPointCount = 6;

		private readonly List<string> _progressNames;
		private readonly List<int> _progressPointValues;

		public Progress(List<int> progressPointValues, List<string> progressNames)
		{
			_progressNames = progressNames;
			_progressPointValues = progressPointValues;
		}

		public List<Tuple<string, int>> GetAll()
		{
			List<Tuple<string, int>> progress = new();

			for (int i = 0; i < _progressNames.Count; i++)
				progress.Add(new Tuple<string, int>(_progressNames[i], _progressPointValues[i]));

			return progress;
		}

		public Tuple<string, int> GetByName(string name)
		{
			if (_progressNames == null || _progressNames.Count == 0)
				throw new NullReferenceException("Progress is not initialized");

			for (int i = 0; i < _progressNames.Count; i++)
				if (_progressNames[i] == name)
					return new Tuple<string, int>(_progressNames[i], _progressPointValues[i]);

			throw new InvalidOperationException("Invalid progress name: " + name);
		}

		public void ChangeValue(string progressName, int progressValue)
		{
			if (_progressNames.Contains(progressName) == false)
				return;

			int nameIndex = _progressNames.FindIndex(x => x == progressName);

			if (nameIndex >= 0)
				_progressPointValues[nameIndex] = progressValue;
		}
	}
}