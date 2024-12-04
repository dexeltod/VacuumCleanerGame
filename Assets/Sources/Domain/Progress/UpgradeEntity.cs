using System;
using Sources.Domain.Progress.Entities.Values;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.DomainInterfaces.Models.Shop.Upgrades;
using UnityEngine;

namespace Sources.Domain.Progress
{
	[Serializable] public class UpgradeEntity : IUpgradeEntity, IUpgradeEntityReadOnly
	{
		[SerializeField] private int _configId;
		[SerializeField] private IntEntityValue _currentLevel;

		public UpgradeEntity(IntEntityValue currentLevel, int configId)
		{
			if (configId < 0) throw new ArgumentOutOfRangeException(nameof(configId));

			_currentLevel = currentLevel ?? throw new ArgumentNullException(nameof(currentLevel));
			_configId = configId;
		}

		public int ConfigId => _configId;
		public IReadOnlyProgressValue<int> LevelProgress => _currentLevel;
		public int Value => _currentLevel.Value;

		public void AddOneLevel() =>
			_currentLevel.Set(_currentLevel.Value + 1);
	}
}