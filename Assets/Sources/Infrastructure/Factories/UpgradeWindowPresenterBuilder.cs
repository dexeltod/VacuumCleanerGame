using System;
using Sources.DomainInterfaces;
using Sources.Presentation;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.UI;
using Sources.Utils.Configs.Scripts;

namespace Sources.Application.StateMachine.GameStates
{
	public class UpgradeWindowPresenterBuilder
	{
		private readonly IAssetProvider _assetProvider;
		private readonly IUpgradeWindow _upgradeWindow;
		private readonly IProgressLoadDataService _progressLoadDataService;
		private readonly IUpgradeWindowPresenter _upgradeWindowPresenter;
		private string GameObjectsUpgradeTrigger => ResourcesAssetPath.GameObjects.UpgradeTrigger;

		public UpgradeWindowPresenterBuilder(
			IAssetProvider assetProvider,
			IUpgradeWindow upgradeWindow,
			IProgressLoadDataService progressLoadDataService,
			IUpgradeWindowPresenter upgradeWindowPresenter
		)
		{
			_assetProvider = assetProvider ?? throw new ArgumentNullException(nameof(assetProvider));
			_upgradeWindow = upgradeWindow ?? throw new ArgumentNullException(nameof(upgradeWindow));
			_progressLoadDataService = progressLoadDataService ??
				throw new ArgumentNullException(nameof(progressLoadDataService));
			_upgradeWindowPresenter = upgradeWindowPresenter ??
				throw new ArgumentNullException(nameof(upgradeWindowPresenter));
		}

		public IUpgradeWindowPresenter Build()
		{
			UpgradeTriggerObserver upgradeTrigger = _assetProvider.InstantiateAndGetComponent<UpgradeTriggerObserver>(
				GameObjectsUpgradeTrigger
			);

			_upgradeWindowPresenter.Initialize(
				upgradeTrigger,
				_upgradeWindow,
				_progressLoadDataService
			);

			return _upgradeWindowPresenter;
		}
	}
}