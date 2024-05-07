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

		public ILevelProgress LevelProgress
		{
			get
			{
				if (_levelProgress != null) return _levelProgress;
				throw new NullReferenceException("LevelProgress is null");
			}
		}

		public IResourceModel ResourceModelReadOnly
		{
			get
			{
				if (_resource != null)
					return _resource;

				throw new NullReferenceException("IResourceModelReadOnly is null");
			}
		}

		public IShopModel ShopModel
		{
			get
			{
				if (_shopModel != null) return _shopModel;
				throw new NullReferenceException("IShopModel is null");
			}
		}

		public IPlayerModel PlayerModel
		{
			get
			{
				if (_playerModel != null) return _playerModel;
				throw new NullReferenceException("IPlayerModel is null");
			}
		}

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
			_playerModel = playerModel ?? throw new ArgumentNullException(nameof(playerModel));
		}

		public bool IsAllProgressNotNull() =>
			_levelProgress != null &&
			_shopModel != null &&
			_playerModel != null &&
			_resource != null &&
			_shopModel.ProgressEntities != null;
	}
}