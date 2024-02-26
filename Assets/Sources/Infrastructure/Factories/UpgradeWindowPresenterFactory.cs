using System;
using Sources.Controllers;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Common.Factory.Decorators;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Providers;
using Sources.PresentationInterfaces;
using Sources.Services.Triggers;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs.Scripts;

namespace Sources.Infrastructure.Factories
{
	public class UpgradeWindowPresenterFactory : PresenterFactory<UpgradeWindowPresenter>
	{
		private readonly IUpgradeWindowViewFactory _upgradeWindowViewFactory;
		private readonly IAssetFactory _assetFactory;
		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
		private readonly IPersistentProgressService _persistentProgressService;
		private readonly IGameplayInterfacePresenter _gameplayInterfacePresenter;
		private readonly IResourcesProgressPresenterProvider _resourcesProgressPresenterProvider;
		private readonly IUpgradeWindowPresenter _upgradeWindowPresenter;

		public UpgradeWindowPresenterFactory(
			IUpgradeWindowViewFactory upgradeWindowViewFactory,
			IAssetFactory assetFactory,
			IProgressSaveLoadDataService progressSaveLoadDataService,
			IPersistentProgressService persistentProgressService,
			IGameplayInterfacePresenter gameplayInterfacePresenter,
			IResourcesProgressPresenterProvider resourcesProgressPresenterProvider
		)
		{
			_upgradeWindowViewFactory
				= upgradeWindowViewFactory ?? throw new ArgumentNullException(nameof(upgradeWindowViewFactory));
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_progressSaveLoadDataService = progressSaveLoadDataService ??
				throw new ArgumentNullException(nameof(progressSaveLoadDataService));
			_persistentProgressService = persistentProgressService ??
				throw new ArgumentNullException(nameof(persistentProgressService));
			_gameplayInterfacePresenter = gameplayInterfacePresenter ??
				throw new ArgumentNullException(nameof(gameplayInterfacePresenter));
			_resourcesProgressPresenterProvider = resourcesProgressPresenterProvider ??
				throw new ArgumentNullException(nameof(resourcesProgressPresenterProvider));
		}

		private IResourcesModel GameProgressResourcesModel => _persistentProgressService.GlobalProgress.ResourcesModel;

		private int SoftCurrencyCount => GameProgressResourcesModel.SoftCurrency.Count;

		private string GameObjectsUpgradeTrigger => ResourcesAssetPath.GameObjects.UpgradeTrigger;

		public override UpgradeWindowPresenter Create()
		{
			IUpgradeWindow upgradeWindow = _upgradeWindowViewFactory.Create();

			UpgradeTriggerObserver upgradeTrigger = _assetFactory.InstantiateAndGetComponent<UpgradeTriggerObserver>(
				GameObjectsUpgradeTrigger
			);

			UpgradeWindowPresenter presenter = new(
				upgradeTrigger,
				upgradeWindow,
				_progressSaveLoadDataService,
				_gameplayInterfacePresenter,
				_resourcesProgressPresenterProvider
			);

			upgradeWindow.Construct(presenter, SoftCurrencyCount);

			return presenter;
		}
	}
}