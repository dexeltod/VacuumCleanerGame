using System;
using Sources.ControllersInterfaces;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Repository;
using Sources.Utils.Enums;

namespace Sources.Controllers.Shop
{
	public class ShopPurchaseController
	{
		private readonly IProgressService _progressService;
		private readonly IResourcesProgressPresenter _resourcesProgressPresenter;
		private readonly IPlayerModelRepository _playerModelRepository;

		public ShopPurchaseController(
			IProgressService progressService,
			IResourcesProgressPresenter resourcesProgressPresenter,
			IPlayerModelRepository playerModelRepository
		)
		{
			_progressService = progressService ?? throw new ArgumentNullException(nameof(progressService));
			_resourcesProgressPresenter = resourcesProgressPresenter;
			_playerModelRepository
				= playerModelRepository ?? throw new ArgumentNullException(nameof(playerModelRepository));
		}

		public bool TryAddOneProgressPoint(int id)
		{
			var price = _progressService.GetPrice(id);

			if (_resourcesProgressPresenter.DecreaseMoney(price) == false)
				return false;

			_progressService.AddProgressPoint(id);

			_playerModelRepository.Set((ProgressType)id, _progressService.GetProgressStatValue(id));
			return true;
		}
	}
}