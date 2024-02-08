using System;
using Cysharp.Threading.Tasks;
using Sources.Domain.Progress;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.DTO;
using VContainer;

namespace Sources.Infrastructure.Shop
{
	public class ShopProgressProvider : IShopProgressProvider
	{
		private const int Point = 1;
		private readonly IGameProgress _shopProgress;
		private readonly IProgressSaveLoadDataService _progressSaveLoadService;

		[Inject]
		public ShopProgressProvider(
			IPersistentProgressService shopProgress,
			IProgressSaveLoadDataService progressSaveLoadDataService
		)
		{
			_shopProgress = shopProgress.GameProgress.ShopProgress;
			_progressSaveLoadService = progressSaveLoadDataService;
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

			await _progressSaveLoadService.SaveToCloud();
			succeededCallback.Invoke();
		}
	}
}