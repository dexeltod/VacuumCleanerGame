using System;
using Sources.Domain.Progress.Player;
using Sources.DomainInterfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.Domain.Progress
{
	[Serializable] public class GameProgressProvider : IGameProgressProvider
	{
		[SerializeField] private ResourcesModel _resources;
		[SerializeField] private PlayerProgress _playerProgress;
		[SerializeField] private UpgradeProgressModel _upgradeProgressModel;
		[SerializeField] private LevelProgress _levelProgress;

		public IGameProgress ShopProgress => _upgradeProgressModel;
		public IGameProgress LevelProgress => _levelProgress;
		public IGameProgress PlayerProgress => _playerProgress;
		public IResourcesModel ResourcesModel => _resources;

		public GameProgressProvider(
			ResourcesModel resourcesModel,
			PlayerProgress playerProgress,
			UpgradeProgressModel upgradeProgressModel,
			LevelProgress levelProgress
		)
		{
			_resources = resourcesModel;
			_playerProgress = playerProgress;
			_upgradeProgressModel = upgradeProgressModel;
			_levelProgress = levelProgress;
		}
	}
}