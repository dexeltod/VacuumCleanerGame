using System;
using System.Linq;
using Sources.BusinessLogic.Repository;
using Sources.BusinessLogic.Services;
using Sources.DomainInterfaces;
using Sources.Utils.Enums;
using UnityEngine;

namespace Sources.Infrastructure.Repository
{
	public class ShopService : IShopService
	{
		private readonly IPlayerModelRepository _playerModelRepository;
		private readonly IProgressEntityRepository _progressRepository;
		private readonly IResourceModel _resourceModel;

		public ShopService(
			IProgressEntityRepository progressRepository,
			IPlayerModelRepository playerModelRepository,
			IResourceModel resourceModel
		)
		{
			_progressRepository = progressRepository ?? throw new ArgumentNullException(nameof(progressRepository));
			_playerModelRepository
				= playerModelRepository ?? throw new ArgumentNullException(nameof(playerModelRepository));
			_resourceModel = resourceModel ?? throw new ArgumentNullException(nameof(resourceModel));
		}

		public bool TryAddOneProgressPoint(int id)
		{
			int price = _progressRepository.GetPrice(id);

			if (_resourceModel.TryDecreaseMoney(price) == false)
				return false;

			float statByProgress = _progressRepository.GetStatByProgress(id);

			_playerModelRepository.Set((ProgressType)id, statByProgress);

			if (!CanAddOneProgressPoint(id))
				return false;

			_progressRepository.AddOneLevel(id);

			return true;
		}

		private bool CanAddOneProgressPoint(int id)
		{
			int count = _progressRepository.GetConfig(id).Items.Count();
			int value = _progressRepository.GetEntity(id).Value;

			Debug.Log(_progressRepository.GetEntity(id).LevelProgress.ReadOnlyValue);

			Debug.Log($"value <= count: {value < count}");
			return value < count;
		}
	}
}
