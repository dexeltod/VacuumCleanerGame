using System;
using Cysharp.Threading.Tasks;
using Sources.DIService;
using Sources.Domain.Progress;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.DTO;
using Sources.ServicesInterfaces;

namespace Sources.Infrastructure.Shop
{
	public class ShopProgressProvider : IShopProgressProvider
	{
		private const    int                      Point = 1;
		private readonly IGameProgress            _shopProgress;
		private readonly IProgressLoadDataService _progressLoadService;

		public ShopProgressProvider(IGameProgress shopProgress, IProgressLoadDataService progressLoadDataService)
		{
			_shopProgress = shopProgress;

			_progressLoadService = progressLoadDataService; 
		}

		public async UniTask AddProgressPoint(string progressName, Action succeededCallback)
		{
			IUpgradeProgressData upgradeProgress = _shopProgress.GetByName(progressName);

			if (upgradeProgress == null)
				throw new NullReferenceException("Progress is null");

			int newProgressPoint = upgradeProgress.Value + Point;

			if (newProgressPoint > _shopProgress.MaxUpgradePointCount)
				return;

			_shopProgress.SetProgress(progressName, newProgressPoint);

			await _progressLoadService.SaveToCloud();
			succeededCallback.Invoke();
		}
	}
}