using System;
using Sources.Domain.Player;
using Sources.Domain.Progress.Player;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.Models;
using Sources.DomainInterfaces.Models.Shop;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.Domain.Progress
{
	[Serializable]
	public class GlobalProgress : IGlobalProgress
	{
		[SerializeField] private ResourceModel _resource;

		[SerializeField] private LevelProgress _levelProgress;
		[SerializeField] private ShopModel _shopModel;

		[FormerlySerializedAs("_playerModel")]
		[SerializeField]
		private PlayerStatsModel _playerStatsModel;

		public ILevelProgress LevelProgress => _levelProgress;
		public IResourceModel ResourceModelReadOnly => _resource;
		public IShopModel ShopModel => _shopModel;
		public IPlayerStatsModel PlayerStatsModel => _playerStatsModel;

		public bool Validate() =>
			_resource != null &&
			_levelProgress != null &&
			_shopModel != null &&
			_playerStatsModel != null &&
			_shopModel.ProgressEntities != null;

		public GlobalProgress(ResourceModel resourceModel,
			LevelProgress levelProgress,
			ShopModel shopModel,
			PlayerStatsModel playerStatsModel
		)
		{
			_resource = resourceModel ?? throw new ArgumentNullException(nameof(resourceModel));

			_levelProgress = levelProgress ?? throw new ArgumentNullException(nameof(levelProgress));

			_shopModel = shopModel ?? throw new ArgumentNullException(nameof(shopModel));

			if (shopModel.ProgressEntities == null)
				throw new ArgumentNullException(nameof(shopModel.ProgressEntities));

			_playerStatsModel = playerStatsModel ?? throw new ArgumentNullException(nameof(playerStatsModel));
		}
	}
}