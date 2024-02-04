using System;
using Sources.Controllers.Common;
using Sources.DomainInterfaces;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;

namespace Sources.Controllers
{
	public class UpgradeWindowPresenter : Presenter, IDisposable
	{
		private IUpgradeWindow _upgradeWindow;
		private IProgressLoadDataService _progressLoadService;
		private IUpgradeTriggerObserver _observer;

		private bool _isCanSave;

		public UpgradeWindowPresenter(
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

		public override void Enable() =>
			_observer.TriggerEntered += OnTriggerEnter;

		public override void Disable() =>
			_observer.TriggerEntered -= OnTriggerEnter;

		private async void OnTriggerEnter(bool isEntered)
		{
			_upgradeWindow.SetActiveYesNoButtons(isEntered);

			if (isEntered != false || _isCanSave == false)
				return;

			await _progressLoadService.SaveToCloud(() => _isCanSave = true);
		}

		public void Dispose() =>
			Disable();
	}
}