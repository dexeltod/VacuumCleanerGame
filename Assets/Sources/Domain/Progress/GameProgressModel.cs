using System;
using Sources.Domain.Progress.Player;
using Sources.DomainInterfaces;
using UnityEngine;

namespace Sources.Domain.Progress
{
	[Serializable]
	public class GameProgressModel : IGameProgressModel
	{
		[SerializeField] private ShopProgress _shopProgress;
		[SerializeField] private PlayerProgress _playerProgress;
		[SerializeField] private Player.ResourcesData _resourcesData;
		[SerializeField] public IGameProgress ShopProgress => _shopProgress;
		[SerializeField] public IGameProgress PlayerProgress => _playerProgress;
		[SerializeField] public IResourcesData ResourcesData => _resourcesData;

		public GameProgressModel(Player.ResourcesData resourcesData, PlayerProgress playerProgress,
			ShopProgress shopProgress)
		{
			_resourcesData = resourcesData;
			_playerProgress = playerProgress;
			_shopProgress = shopProgress;
		}
	}
}