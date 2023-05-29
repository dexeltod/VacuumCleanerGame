using System;
using System.Collections.Generic;

namespace Model.Infrastructure.Data
{
	[Serializable]
	public class ShopProgress
	{
		private List<string> _progressNames = new();

		private List<int> _progressValues;

		public int BoughtSpeedPoints { get; private set; } = 1;

		private bool _namesInitialized;

		public ShopProgress()
		{
			_namesInitialized = false;
		}

		public void InitializeProgress(List<string> progressNames)
		{
			if (_namesInitialized)
				return;

			_progressNames.Clear();
			
			_progressValues = new List<int>();
			

			for (int i = 0; i < progressNames.Count; i++)
			{
				_progressNames.Add(progressNames[i]);
				_progressValues.Add(0);
			}

			_namesInitialized = true;
		}

		public List<Tuple<string, int>> LoadProgress()
		{
			List<Tuple<string, int>> progress = new();

			for (int i = 0; i < _progressNames.Count; i++)
				progress.Add(new Tuple<string, int>(_progressNames[i], _progressValues[i]));

			return progress;
		}

		public void SetBoughtSpeedPoints(int newBoughtSpeedPoints) => BoughtSpeedPoints = newBoughtSpeedPoints;

		public void ChangeProgressValue(string progressName, int progressValue)
		{
			if (_progressNames.Contains(progressName) == false)
				return;

			int index = _progressNames.FindIndex(x => x == progressName);

			if (index >= 0)
				_progressValues[index] = progressValue;
		}
	}
}