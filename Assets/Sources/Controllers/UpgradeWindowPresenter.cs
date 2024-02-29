using System;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces.Providers;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;

namespace Sources.Controllers
{
	public class UpgradeWindowPresenter : Presenter, IDisposable, IUpgradeWindowPresenter
	{
		private readonly IUpgradeWindow _upgradeWindow;
		private readonly IProgressSaveLoadDataService _progressSaveLoadService;
		private readonly IGameplayInterfacePresenter _gameplayInterfacePresenter;
		private readonly IResourcesProgressPresenterProvider _resourcesProgressPresenterProvider;
		private readonly IUpgradeTriggerObserver _observer;

		private bool _isCanSave;

		public UpgradeWindowPresenter(
			IUpgradeTriggerObserver observer,
			IUpgradeWindow upgradeWindow,
			IProgressSaveLoadDataService progressSaveLoadDataService,
			IGameplayInterfacePresenter gameplayInterfacePresenter,
			IResourcesProgressPresenterProvider resourcesProgressPresenterProvider
		)
		{
			_observer = observer ?? throw new ArgumentNullException(nameof(observer));
			_upgradeWindow = upgradeWindow ?? throw new ArgumentNullException(nameof(upgradeWindow));
			_progressSaveLoadService = progressSaveLoadDataService ??
				throw new ArgumentNullException(nameof(progressSaveLoadDataService));
			_gameplayInterfacePresenter = gameplayInterfacePresenter ??
				throw new ArgumentNullException(nameof(gameplayInterfacePresenter));
			_resourcesProgressPresenterProvider = resourcesProgressPresenterProvider ??
				throw new ArgumentNullException(nameof(resourcesProgressPresenterProvider));
		}

		private int SoftCurrencyCount => _resourcesProgressPresenterProvider.Implementation.SoftCurrency.Count;

		public override void Enable()
		{
			_gameplayInterfacePresenter.Enable();
			_upgradeWindow.SetMoney(SoftCurrencyCount);
			_observer.TriggerEntered += OnTriggerEnter;
		}

		public override void Disable()
		{
			_gameplayInterfacePresenter.Disable();
			_observer.TriggerEntered -= OnTriggerEnter;
		}

		public void SetMoney(int money) =>
			_upgradeWindow.SetMoney(money);

		private async void OnTriggerEnter(bool isEntered)
		{
			_upgradeWindow.SetActiveYesNoButtons(isEntered);

			if (isEntered != false || _isCanSave == false)
				return;

			await _progressSaveLoadService.SaveToCloud(() => _isCanSave = true);
		}

		public void Dispose() =>
			Disable();
	}
}