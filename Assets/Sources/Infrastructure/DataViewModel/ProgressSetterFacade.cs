using System;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces.Providers;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.Upgrade;

namespace Sources.Infrastructure.DataViewModel
{
	public class ProgressSetterFacade : IProgressSetterFacade
	{
		private const int OnePoint = 1;

		private readonly IPlayerStatsServiceProvider _playerStatsProvider;
		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
		private readonly IPersistentProgressServiceProvider _persistentProgressServiceProvider;
		private readonly IResourcesProgressPresenterProvider _resourcesProgressPresenterProvider;
		private readonly IPersistentProgressService _persistentProgressService;

		public ProgressSetterFacade(
			IPlayerStatsServiceProvider playerStats,
			IProgressSaveLoadDataService persistentProgressService,
			IPersistentProgressServiceProvider persistentProgressServiceProvider,
			IResourcesProgressPresenterProvider resourcesProgressPresenterProvider
		)
		{
			_playerStatsProvider = playerStats ?? throw new ArgumentNullException(nameof(playerStats));
			_progressSaveLoadDataService = persistentProgressService ??
				throw new ArgumentNullException(nameof(persistentProgressService));
			_persistentProgressServiceProvider = persistentProgressServiceProvider ??
				throw new ArgumentNullException(nameof(persistentProgressServiceProvider));
			_resourcesProgressPresenterProvider = resourcesProgressPresenterProvider ??
				throw new ArgumentNullException(nameof(resourcesProgressPresenterProvider));
		}

		private IResourcesProgressPresenter ResourcesProgressPresenter =>
			_resourcesProgressPresenterProvider.Implementation;

		private IGameProgress PlayerProgress =>
			_persistentProgressServiceProvider.Implementation.GlobalProgress.PlayerProgress;

		private IGameProgress ShopProgress =>
			_persistentProgressServiceProvider.Implementation.GlobalProgress.ShopProgress;

		private IPlayerStatsService PlayerStatsService => _playerStatsProvider.Implementation;

		public bool TryAddOneProgressPoint(string progressName, IUpgradeItemData itemData)
		{
			if (itemData == null) throw new ArgumentNullException(nameof(itemData));

			IUpgradeProgressData upgradeProgress = PlayerProgress.GetByName(progressName);

			int newProgressValue = upgradeProgress.Value + OnePoint;

			if (newProgressValue > PlayerProgress.MaxUpgradePointCount)
				return false;

			PlayerStatsService.Set(upgradeProgress.Name, newProgressValue);
			PlayerProgress.Set(progressName, newProgressValue);
			ShopProgress.Set(progressName, newProgressValue);

			ResourcesProgressPresenter.DecreaseMoney(itemData.Price);

			itemData.SetUpgradeLevel(upgradeProgress.Value);

			_progressSaveLoadDataService.SaveToCloud();

			return true;
		}
	}
}