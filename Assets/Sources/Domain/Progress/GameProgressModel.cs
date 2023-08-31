using System;
using Newtonsoft.Json;
using Sources.Domain.Progress.Player;
using Sources.DomainInterfaces;
using UnityEngine;

namespace Sources.Domain.Progress
{
	[Serializable]
	public class GameProgressModel : IGameProgressModel
	{
		[SerializeField] private ResourcesModel _resources;
		[SerializeField] private PlayerProgress _playerProgress;
		[SerializeField] private ShopProgress _shopProgress;

		public IGameProgress ShopProgress => _shopProgress;
		public IGameProgress PlayerProgress => _playerProgress;
		public IResourcesModel ResourcesModel => _resources;

		[JsonConstructor]
		public GameProgressModel(ResourcesModel resourcesModel, PlayerProgress playerProgress,
			ShopProgress shopProgress)
		{
			_resources = resourcesModel;
			_playerProgress = playerProgress;
			_shopProgress = shopProgress;
		}
	}
}