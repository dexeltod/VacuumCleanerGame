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
	[Serializable] public class GlobalProgress : IGlobalProgress
	{
		[SerializeField] private ResourceModel _resource;

		[SerializeField] private LevelProgress _levelProgress;
		[SerializeField] private ShopModel _shopModel;

		[SerializeField] private PlayerModel _playerModel;

		public ILevelProgress LevelProgress => _levelProgress;
		public IResourceModelReadOnly ResourceModelReadOnly => _resource;
		public IShopModel ShopModel => _shopModel;
		public IPlayerModel PlayerModel => _playerModel;

		public bool Validate() =>
			_resource != null &&
			_levelProgress != null &&
			_shopModel != null &&
			_playerModel != null &&
			_shopModel.ProgressEntities != null;

		public GlobalProgress(
			ResourceModel resourceModel,
			LevelProgress levelProgress,
			ShopModel shopModel,
			PlayerModel playerModel
		)
		{
			_resource = resourceModel ??
				throw new ArgumentNullException(nameof(resourceModel));

			_levelProgress = levelProgress ?? throw new ArgumentNullException(nameof(levelProgress));

			_shopModel = shopModel ?? throw new ArgumentNullException(nameof(shopModel));

			if (shopModel.ProgressEntities == null)
				throw new ArgumentNullException(nameof(shopModel.ProgressEntities));

			_playerModel = playerModel ?? throw new ArgumentNullException(nameof(playerModel));
		}
	}
}