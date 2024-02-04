using System;
using Sources.Controllers;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories.Common.Decorators;
using Sources.InfrastructureInterfaces.Factory;
using Sources.PresentationInterfaces;
using Sources.PresentersInterfaces;
using Sources.Services.Triggers;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs.Scripts;

namespace Sources.Infrastructure.Factories
{
	public class UpgradeWindowPresenterBuilder : PresenterFactory<UpgradeWindowPresenter>
	{
		private readonly IUpgradeWindowFactory _upgradeWindowFactory;
		private readonly IAssetResolver _assetResolver;
		private readonly IProgressLoadDataService _progressLoadDataService;
		private readonly IUpgradeWindowPresenter _upgradeWindowPresenter;

		private string GameObjectsUpgradeTrigger => ResourcesAssetPath.GameObjects.UpgradeTrigger;

		public UpgradeWindowPresenterBuilder(
			IUpgradeWindowFactory upgradeWindowFactory,
			IAssetResolver assetResolver,
			IProgressLoadDataService progressLoadDataService
		)
		{
			_upgradeWindowFactory
				= upgradeWindowFactory ?? throw new ArgumentNullException(nameof(upgradeWindowFactory));
			_assetResolver = assetResolver ?? throw new ArgumentNullException(nameof(assetResolver));
			_progressLoadDataService = progressLoadDataService ??
				throw new ArgumentNullException(nameof(progressLoadDataService));
		}

		public override UpgradeWindowPresenter Create()
		{
			IUpgradeWindow upgradeWindow = _upgradeWindowFactory.Create();

			UpgradeTriggerObserver upgradeTrigger = _assetResolver.InstantiateAndGetComponent<UpgradeTriggerObserver>(
				GameObjectsUpgradeTrigger
			);

			return new UpgradeWindowPresenter(
				upgradeTrigger,
				upgradeWindow,
				_progressLoadDataService
			);
		}
	}
}