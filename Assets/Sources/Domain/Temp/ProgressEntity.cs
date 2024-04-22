using System;
using UnityEngine;

namespace Sources.Domain.Temp

{
	[Serializable] public class ProgressEntity : IProgressEntity
	{
		[SerializeField] private int _currentLevel;
		[SerializeField] private int _configId;

		public ProgressEntity(int currentLevel, int configId)
		{
			if (currentLevel < 0) throw new ArgumentOutOfRangeException(nameof(currentLevel));
			if (configId < 0) throw new ArgumentOutOfRangeException(nameof(configId));

			_currentLevel = currentLevel;
			_configId = configId;
		}

		public int ConfigId => _configId;
		public int CurrentLevel => _currentLevel;

		public event Action LevelChanged;
		public event Action PriceChanged;

		public void AddOneLevel()
		{
			_currentLevel++;
			PriceChanged!.Invoke();
			LevelChanged!.Invoke();
		}
	}
}