using System;
using Sources.Domain.Progress.Player;
using Sources.DomainInterfaces;
using UnityEngine;

namespace Sources.Domain.Progress
{
	[Serializable] public class GlobalProgress : IGlobalProgress
	{
		[SerializeField] private ResourcesModel _resources;
		[SerializeField] private PlayerProgress _playerProgress;
		[SerializeField] private UpgradeProgressModel _upgradeProgressModelModel;
		[SerializeField] private LevelProgress _levelProgress;

		public IGameProgress UpgradeProgressModel => _upgradeProgressModelModel;
		public ILevelProgress LevelProgress => _levelProgress;
		public IGameProgress PlayerProgress => _playerProgress;
		public IResourcesModel ResourcesModel => _resources;

		public GlobalProgress(
			ResourcesModel resourcesModel,
			PlayerProgress playerProgress,
			UpgradeProgressModel upgradeProgressModelModel,
			LevelProgress levelProgress
		)
		{
			_resources = resourcesModel;
			_playerProgress = playerProgress;
			_upgradeProgressModelModel = upgradeProgressModelModel;
			_levelProgress = levelProgress;
		}
	}
}