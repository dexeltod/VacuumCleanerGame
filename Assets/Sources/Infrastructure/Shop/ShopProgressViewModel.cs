using System;
using Sources.Core.DI;
using Sources.Core.Domain.Progress;
using Sources.DomainServices;
using Sources.DomainServices.Interfaces;
using Sources.Infrastructure.InfrastructureInterfaces;

namespace Sources.Infrastructure.Shop
{
	public class ShopProgressViewModel : IShopProgressViewModel
	{
		private const int Point = 1;
		private readonly ShopProgress _shopProgress;
		private readonly ISaveLoadDataService _saveLoadService;

		public ShopProgressViewModel()
		{
			var gameProgress = ServiceLocator.Container.GetSingle<IPersistentProgressService>().GameProgress;
			_saveLoadService = ServiceLocator.Container.GetSingle<ISaveLoadDataService>();
			_shopProgress = gameProgress.ShopProgress;
		}
		
		public void AddProgressPoint(string progressName)
		{
			Tuple<string, int> progress = _shopProgress.GetByName(progressName);

			if (progress == null)
				throw new NullReferenceException("Progress is null");

			int newProgressPoint = progress.Item2 + Point;

			if (newProgressPoint > _shopProgress.MaxPointCount)
				return;

			_shopProgress.ChangeValue(progressName, newProgressPoint);
			_saveLoadService.SaveProgress();
		}
	}
}