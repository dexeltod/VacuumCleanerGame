using System;
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
		private const int Point = 1;
		private readonly IGameProgress _shopProgress;
		private readonly ISaveLoadDataService _saveLoadService;

		public ShopProgressProvider()
		{
			var gameProgress = GameServices.Container.Get<IPersistentProgressService>().GameProgress;
			_saveLoadService = GameServices.Container.Get<ISaveLoadDataService>();
			_shopProgress = gameProgress.ShopProgress;
		}

		public void AddProgressPoint(string progressName)
		{
			IUpgradeProgressData upgradeProgress = _shopProgress.GetByName(progressName);

			if (upgradeProgress == null)
				throw new NullReferenceException("Progress is null");

			int newProgressPoint = upgradeProgress.Value + Point;

			if (newProgressPoint > _shopProgress.MaxPointCount)
				return;

			_shopProgress.SetProgress(progressName, newProgressPoint);
#if !YANDEX_GAMES
			_saveLoadService.SaveToUnityCloud();
#endif
		}
	}
}