using System;
using Sources.Domain.Player;
using Sources.Domain.Progress.Player;
using Sources.Domain.Settings;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.Entities;
using Sources.DomainInterfaces.Models;
using Sources.DomainInterfaces.Models.Shop;
using UnityEngine;

namespace Sources.Domain.Progress
{
	[Serializable]
	public class GlobalProgress : IGlobalProgress
	{
		[SerializeField] private ResourceModel _resource;
		[SerializeField] private LevelProgress _levelProgress;
		[SerializeField] private ShopModel _shopModel;
		[SerializeField] private SoundSettings _soundSettings;
		[SerializeField] private PlayerStatsModel _playerStatsModel;

		public GlobalProgress(
			ResourceModel resourceModel,
			LevelProgress levelProgress,
			ShopModel shopModel,
			PlayerStatsModel playerStatsModel,
			SoundSettings soundSettings
		)
		{
			_resource = resourceModel ?? throw new ArgumentNullException(nameof(resourceModel));

			_levelProgress = levelProgress ?? throw new ArgumentNullException(nameof(levelProgress));

			_shopModel = shopModel ?? throw new ArgumentNullException(nameof(shopModel));

			if (shopModel.ProgressEntities == null)
				throw new ArgumentNullException(nameof(shopModel.ProgressEntities));

			_soundSettings = soundSettings ?? throw new ArgumentNullException(nameof(soundSettings));
			_playerStatsModel = playerStatsModel ?? throw new ArgumentNullException(nameof(playerStatsModel));
		}

		public ILevelProgress LevelProgress => _levelProgress;
		public IResourceModel ResourceModel => _resource;
		public IShopModel ShopModel => _shopModel;
		public IPlayerStatsModel PlayerStatsModel => _playerStatsModel;
		public ISoundSettings SoundSettings => _soundSettings;

		public bool Validate() =>
			_resource != null
			&& _levelProgress != null
			&& _shopModel != null
			&& _playerStatsModel != null
			&& _shopModel.ProgressEntities != null;
	}
}
