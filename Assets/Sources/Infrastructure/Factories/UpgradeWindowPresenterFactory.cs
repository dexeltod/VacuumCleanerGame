using System;
using Sources.Controllers;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Common.Factory.Decorators;
using Sources.InfrastructureInterfaces.Factory;
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
		private readonly IUpgradeWindowPresenter _upgradeWindowPresenter;
		private IResourcesModel GameProgressResourcesModel => _persistentProgressService.GlobalProgress.ResourcesModel;

		private string GameObjectsUpgradeTrigger => ResourcesAssetPath.GameObjects.UpgradeTrigger;

		public UpgradeWindowPresenterFactory(
			IUpgradeWindowViewFactory upgradeWindowViewFactory,
			IAssetFactory assetFactory,
			IProgressSaveLoadDataService progressSaveLoadDataService,
			IPersistentProgressService persistentProgressService
		)
		{
			_upgradeWindowViewFactory
				= upgradeWindowViewFactory ?? throw new ArgumentNullException(nameof(upgradeWindowViewFactory));
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_progressSaveLoadDataService = progressSaveLoadDataService ??
				throw new ArgumentNullException(nameof(progressSaveLoadDataService));
			_persistentProgressService = persistentProgressService ??
				throw new ArgumentNullException(nameof(persistentProgressService));
		}

		public override UpgradeWindowPresenter Create()
		{
			IUpgradeWindow upgradeWindow = _upgradeWindowViewFactory.Create();

			UpgradeTriggerObserver upgradeTrigger = _assetFactory.InstantiateAndGetComponent<UpgradeTriggerObserver>(
				GameObjectsUpgradeTrigger
			);

			UpgradeWindowPresenter presenter = new(
				upgradeTrigger,
				upgradeWindow,
				_progressSaveLoadDataService
			);

			upgradeWindow.Construct(presenter, GameProgressResourcesModel.CurrentCashScore);

			return presenter;
		}
	}
}