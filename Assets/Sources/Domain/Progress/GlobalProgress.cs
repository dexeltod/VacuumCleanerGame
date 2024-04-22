using System;
using Sources.Domain.Progress.Player;
using Sources.Domain.Temp;
using Sources.DomainInterfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.Domain.Progress
{
	[Serializable] public class GlobalProgress : IGlobalProgress
	{
		[SerializeField] private ResourceModelModifiableReadOnly _resource;

		[SerializeField] private LevelProgress _levelProgress;
		[SerializeField] private ShopEntity _shopEntity;

		public ILevelProgress LevelProgress => _levelProgress;
		public IResourceModelReadOnly ResourceModelReadOnly => _resource;
		public IShopEntity ShopEntity => _shopEntity;

		public GlobalProgress(
			ResourceModelModifiableReadOnly resourceModelModifiableReadOnly,
			LevelProgress levelProgress,
			ShopEntity shopEntity
		)
		{
			_resource = resourceModelModifiableReadOnly;

			_levelProgress = levelProgress;
			_shopEntity = shopEntity;
		}
	}
}