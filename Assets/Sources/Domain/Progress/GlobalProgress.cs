using System;
using Sources.Domain.Player;
using Sources.Domain.Progress.Player;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.Models;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.Domain.Progress
{
	[Serializable] public class GlobalProgress : IGlobalProgress
	{
		[SerializeField] private ResourceModelModifiableReadOnly _resource;

		[SerializeField] private LevelProgress _levelProgress;
		[SerializeField] private ShopModel _shopModel;

		[SerializeField] private PlayerModel _playerModel;

		public ILevelProgress LevelProgress => _levelProgress;
		public IResourceModelReadOnly ResourceModelReadOnly => _resource;
		public IShopModel ShopModel => _shopModel;
		public IPlayerModel PlayerModel => _playerModel;

		public GlobalProgress(
			ResourceModelModifiableReadOnly resourceModelModifiableReadOnly,
			LevelProgress levelProgress,
			ShopModel shopModel,
			PlayerModel playerModel
		)
		{
			_resource = resourceModelModifiableReadOnly ??
				throw new ArgumentNullException(nameof(resourceModelModifiableReadOnly));

			_levelProgress = levelProgress ?? throw new ArgumentNullException(nameof(levelProgress));
			_shopModel = shopModel ?? throw new ArgumentNullException(nameof(shopModel));
			_playerModel = playerModel ?? throw new ArgumentNullException(nameof(playerModel));
		}
	}
}