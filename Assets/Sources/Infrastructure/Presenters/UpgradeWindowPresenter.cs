using System;
using Sources.DomainInterfaces;
using Sources.Presentation;
using Sources.ServicesInterfaces.UI;

namespace Sources.Infrastructure.Presenters
{
	public class UpgradeWindowPresenter : IUpgradeWindowPresenter, IDisposable
	{
		private IUpgradeWindowGetter _upgradeWindowGetter;
		private IUpgradeWindow _upgradeWindow;
		private IProgressLoadDataService _progressLoadService;
		private IUpgradeTriggerObserver _observer;

		private bool _isCanSave;

		public void Initialize(
			IUpgradeTriggerObserver observer,
			IUpgradeWindow upgradeWindow,
			IProgressLoadDataService progressLoadDataService
		)
		{
			_observer = observer ?? throw new ArgumentNullException(nameof(observer));
			_upgradeWindow = upgradeWindow ?? throw new ArgumentNullException(nameof(upgradeWindow));
			_progressLoadService = progressLoadDataService ??
				throw new ArgumentNullException(nameof(progressLoadDataService));
		}

		public void Enable() =>
			_observer.TriggerEntered += OnTriggerEnter;

		private async void OnTriggerEnter(bool isEntered)
		{
			_upgradeWindow.SetActiveYesNoButtons(isEntered);

			if (isEntered != false || _isCanSave == false)
				return;

			await _progressLoadService.SaveToCloud(() => _isCanSave = true);
		}

		public void Disable() =>
			_observer.TriggerEntered -= OnTriggerEnter;

		public void Dispose() =>
			Disable();
	}
}