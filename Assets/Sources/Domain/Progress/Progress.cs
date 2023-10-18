using System;
using System.Collections.Generic;
using System.Linq;
using Sources.DomainInterfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.Domain.Progress
{
	[Serializable] public abstract class Progress : IGameProgress
	{
		[FormerlySerializedAs("_maxPointCount")] [SerializeField]
		private int _maxUpgradePointsCount;

		[SerializeField] private List<ProgressUpgradeData> _progressData;
		[SerializeField] private List<string>              _progressNames;

		public int MaxUpgradePointCount => _maxUpgradePointsCount;

		protected Progress(List<ProgressUpgradeData> progress, int maxUpgradePointsCount = 0)
		{
			_maxUpgradePointsCount = maxUpgradePointsCount;
			_progressData          = progress;
			_progressNames         = new List<string>();

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
				throw new InvalidOperationException("Invalid progress name: " + name);

			return foundedProgress;
		}

		public void SetProgress(string progressName, int progressValue)
		{
			if (_progressNames.Contains(progressName) == false)
				return;

			IUpgradeProgressData element = _progressData.FirstOrDefault(x => x.Name == progressName);

			IUpgradeProgressData upgradeProgress = element;

			if (upgradeProgress == null)
				throw new Exception("upgradeProgress is null");

			upgradeProgress.Value = progressValue;
		}
	}
}