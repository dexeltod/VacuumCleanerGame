using System;
using System.Collections.Generic;
using System.Linq;
using Sources.DomainInterfaces;
using UnityEngine;

namespace Sources.Domain.Progress
{
	[Serializable] public abstract class Progress : IGameProgress
	{
		[SerializeField] private List<ProgressUpgradeData> _progressData;
		[SerializeField] private List<string> _progressNames;

		protected Progress(List<ProgressUpgradeData> progress)
		{
			_progressData = progress ?? throw new ArgumentNullException(nameof(progress));

			_progressNames = new List<string>();

			for (int i = 0; i < progress.Count; i++)
			{
				ProgressUpgradeData progressItem = progress[i];
				_progressNames.Add(progressItem.Name);
				_progressData[i] = progressItem;
			}
		}

		public List<IUpgradeProgressData> GetAll() =>
			_progressData.Cast<IUpgradeProgressData>().ToList();

		public IUpgradeProgressData GetByName(string name)
		{
			if (_progressNames == null || _progressNames.Count == 0)
				throw new NullReferenceException("Progress is not initialized");

			IUpgradeProgressData foundedProgress = _progressData.FirstOrDefault(element => element.Name == name);

			if (foundedProgress == null)
				throw new ArgumentNullException("Invalid progress name: " + name);

			return foundedProgress;
		}

		public void Set(string progressName, int progressValue)
		{
			if (_progressNames.Contains(progressName) == false)
				return;

			IUpgradeProgressData element = _progressData.FirstOrDefault(x => x.Name == progressName);

			if (element == null)
				throw new ArgumentNullException("upgradeProgress is null");

			element.Value = progressValue;
		}
	}
}