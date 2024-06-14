using System;
using Sources.ControllersInterfaces;
using Sources.InfrastructureInterfaces.Repository;
using Sources.Utils.Enums;
using UnityEngine;

namespace Sources.Controllers.Shop
{
	public class ShopPurchaseController
	{
		private readonly IProgressEntityRepository _progressRepository;
		private readonly IResourcesProgressPresenter _resourcesProgressPresenter;
		private readonly IPlayerModelRepository _playerModelRepository;

		public ShopPurchaseController(
			IProgressEntityRepository progressRepository,
			IResourcesProgressPresenter resourcesProgressPresenter,
			IPlayerModelRepository playerModelRepository,
			AudioSource audioSource
		)
		{
			_progressRepository = progressRepository ?? throw new ArgumentNullException(nameof(progressRepository));
			_resourcesProgressPresenter = resourcesProgressPresenter ??
				throw new ArgumentNullException(nameof(resourcesProgressPresenter));
			_playerModelRepository
				= playerModelRepository ?? throw new ArgumentNullException(nameof(playerModelRepository));
		}

		public bool TryAddOneProgressPoint(int id)
		{
			int price = _progressRepository.GetPrice(id);

			if (_resourcesProgressPresenter.TryDecreaseMoney(price) == false)
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
			int count = _progressRepository.GetConfig(id).Stats.Count;
			int value = _progressRepository.GetEntity(id).Value;

			Debug.Log(_progressRepository.GetEntity(id).LevelProgress.Value);

			Debug.Log($"value <= count: {value < count}");
			return value < count;
		}
	}
}