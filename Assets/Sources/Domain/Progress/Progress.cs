using System;
using System.Collections.Generic;
using System.Linq;
using Sources.DomainInterfaces;

namespace Sources.Domain.Progress
{
	[Serializable]
	public abstract class Progress : IGameProgress
	{
		private readonly Dictionary<string, IUpgradeProgressData> _progress;

		public int MaxPointCount { get; private set; } = 6;
		public Progress(List<IUpgradeProgressData> progress)
		{
			_progress = new Dictionary<string, IUpgradeProgressData>();

			foreach (var progressItem in progress)
				_progress.Add(progressItem.Name, progressItem);
		}

		public List<IUpgradeProgressData> GetAll()
		{
			var progress = new List<IUpgradeProgressData>();

			foreach (var value in _progress) 
				progress.Add(value.Value);

			return progress;
		}

		public IUpgradeProgressData GetByName(string name)
		{
			if (_progress == null || _progress.Count == 0)
				throw new NullReferenceException("Progress is not initialized");

			var foundedProgress = _progress.FirstOrDefault(element => element.Key == name);

			if (foundedProgress.Value == null)
				throw new InvalidOperationException("Invalid progress name: " + name);

			return foundedProgress.Value;
		}

		public void SetProgress(string progressName, int progressValue)
		{
			if (_progress.ContainsKey(progressName) == false)
				return;

			KeyValuePair<string, IUpgradeProgressData> element = _progress.FirstOrDefault(x => x.Key == progressName);

			IUpgradeProgressData upgradeProgress = element.Value;
			upgradeProgress.Value = progressValue;
		}
	}
}