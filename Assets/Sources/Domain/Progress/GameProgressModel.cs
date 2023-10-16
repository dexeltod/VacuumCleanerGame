using System;
using System.Collections.Generic;
using Sources.Domain.Progress.Player;
using Sources.DomainInterfaces;
using UnityEngine;

namespace Sources.Domain.Progress
{
	[Serializable] public class GameProgressModel : IGameProgressModel
	{
		[SerializeField] private ResourcesModel _resources;
		[SerializeField] private PlayerProgress _playerProgress;
		[SerializeField] private ShopProgress   _shopProgress;
		[SerializeField] private LevelProgress  _levelProgress;

		public IGameProgress   ShopProgress   => _shopProgress;
		public IGameProgress   LevelProgress  => _levelProgress;
		public IGameProgress   PlayerProgress => _playerProgress;
		public IResourcesModel ResourcesModel => _resources;

		public GameProgressModel
		(
			ResourcesModel resourcesModel,
			PlayerProgress playerProgress,
			ShopProgress   shopProgress,
			LevelProgress  levelProgress
		)
		{
			_resources          = resourcesModel;
			_playerProgress     = playerProgress;
			_shopProgress       = shopProgress;
			_levelProgress = levelProgress;
		}
	}
}