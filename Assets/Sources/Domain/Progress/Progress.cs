using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Sources.DomainInterfaces;
using UnityEngine;

namespace Sources.Domain.Progress
{
	[Serializable]
	public abstract class Progress : IGameProgress
	{
		[SerializeField] private int _maxPointCount = 6;
		[SerializeField] private List<ProgressUpgradeData> _progressData;
		[SerializeField] private List<string> _progressNames;

		public int MaxPointCount => _maxPointCount;

		[JsonConstructor]
		protected Progress(List<ProgressUpgradeData> progress)
		{
			_progressData = progress;
			_progressNames = new List<string>();

			for (int i = 0; i < progress.Count; i++)
			{
				ProgressUpgradeData progressItem = progress[i];
				_progressNames.Add(progressItem.Name);
				_progressData[i] = progressItem;
			}
		}

		public List<IUpgradeProgressData> GetAll()
		{
			List<IUpgradeProgressData> progress = new List<IUpgradeProgressData>();

			foreach (var value in _progressData)
				progress.Add(value);

			return progress;
		}

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
			upgradeProgress.Value = progressValue;
		}
	}
}