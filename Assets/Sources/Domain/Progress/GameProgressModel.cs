using System;
using System.Collections.Generic;
using Sources.Domain.Progress.Player;
using Sources.DomainInterfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.Domain.Progress
{
	[Serializable] public class GameProgressModel : IGameProgressModel
	{
		[SerializeField]                                         private ResourcesModel       _resources;
		[SerializeField]                                         private PlayerProgress       _playerProgress;
		[FormerlySerializedAs("_shopProgress")] [SerializeField] private UpgradeProgressModel _upgradeProgressModel;
		[SerializeField]                                         private LevelProgress        _levelProgress;

		public IGameProgress   ShopProgress   => _upgradeProgressModel;
		public IGameProgress   LevelProgress  => _levelProgress;
		public IGameProgress   PlayerProgress => _playerProgress;
		public IResourcesModel ResourcesModel => _resources;

		public GameProgressModel
		(
			ResourcesModel resourcesModel,
			PlayerProgress playerProgress,
			UpgradeProgressModel   upgradeProgressModel,
			LevelProgress  levelProgress
		)
		{
			_resources          = resourcesModel;
			_playerProgress     = playerProgress;
			_upgradeProgressModel       = upgradeProgressModel;
			_levelProgress = levelProgress;
		}
	}
}