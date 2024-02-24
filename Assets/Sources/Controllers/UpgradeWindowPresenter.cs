using System;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;

namespace Sources.Controllers
{
	public class UpgradeWindowPresenter : Presenter, IDisposable, IUpgradeWindowPresenter
	{
		private readonly IUpgradeWindow _upgradeWindow;
		private readonly IProgressSaveLoadDataService _progressSaveLoadService;
		private readonly IGameplayInterfacePresenter _gameplayInterfacePresenter;
		private readonly IUpgradeTriggerObserver _observer;

		private bool _isCanSave;

		public UpgradeWindowPresenter(
			IUpgradeTriggerObserver observer,
			IUpgradeWindow upgradeWindow,
			IProgressSaveLoadDataService progressSaveLoadDataService,
			IGameplayInterfacePresenter gameplayInterfacePresenter
		)
		{
			_observer = observer ?? throw new ArgumentNullException(nameof(observer));
			_upgradeWindow = upgradeWindow ?? throw new ArgumentNullException(nameof(upgradeWindow));
			_progressSaveLoadService = progressSaveLoadDataService ??
				throw new ArgumentNullException(nameof(progressSaveLoadDataService));
			_gameplayInterfacePresenter = gameplayInterfacePresenter ??
				throw new ArgumentNullException(nameof(gameplayInterfacePresenter));
		}

		public override void Enable() =>
			_observer.TriggerEntered += OnTriggerEnter;

		public override void Disable() =>
			_observer.TriggerEntered -= OnTriggerEnter;

		public void SetMoney(int money) =>
			_upgradeWindow.SetMoney(money);

		private async void OnTriggerEnter(bool isEntered)
		{
			_upgradeWindow.SetActiveYesNoButtons(isEntered);

			if (isEntered)
				_gameplayInterfacePresenter.Disable();
			else
				_gameplayInterfacePresenter.Enable();

			if (isEntered != false || _isCanSave == false)
				return;

			await _progressSaveLoadService.SaveToCloud(() => _isCanSave = true);
		}

		public void Dispose() =>
			Disable();
	}
}