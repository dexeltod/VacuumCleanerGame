using System;
using Cysharp.Threading.Tasks;
using Sources.Domain.Progress;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Providers;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.DTO;
using VContainer;

namespace Sources.Infrastructure.Shop
{
	public class ShopProgressFacade : IShopProgressFacade
	{
		private const int Point = 1;
		private readonly IPersistentProgressServiceProvider _progressService;
		private readonly IProgressSaveLoadDataService _progressSaveLoadService;

		private IGameProgress ShopProgress => _progressService.Implementation.GameProgress.ShopProgress;

		[Inject]
		public ShopProgressFacade(
			IPersistentProgressServiceProvider progressService,
			IProgressSaveLoadDataService progressSaveLoadDataService
		)
		{
			_progressService = progressService ?? throw new ArgumentNullException(nameof(progressService));

			_progressSaveLoadService = progressSaveLoadDataService ??
				throw new ArgumentNullException(nameof(progressSaveLoadDataService));
		}

		public async UniTask AddProgressPoint(string progressName, Action succeededCallback = null)
		{
			IUpgradeProgressData upgradeProgress = ShopProgress.GetByName(progressName);

			if (upgradeProgress == null)
				throw new NullReferenceException("Progress is null");

			int newProgressPoint = upgradeProgress.Value + Point;

			if (newProgressPoint > ShopProgress.MaxUpgradePointCount)
				return;

			ShopProgress.SetProgress(progressName, newProgressPoint);

			await _progressSaveLoadService.SaveToCloud();
			succeededCallback!.Invoke();
		}
	}
}