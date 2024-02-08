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
		private readonly IUpgradeTriggerObserver _observer;

		private bool _isCanSave;

		public UpgradeWindowPresenter(
			IUpgradeTriggerObserver observer,
			IUpgradeWindow upgradeWindow,
			IProgressSaveLoadDataService progressSaveLoadDataService
		)
		{
			_observer = observer ?? throw new ArgumentNullException(nameof(observer));
			_upgradeWindow = upgradeWindow ?? throw new ArgumentNullException(nameof(upgradeWindow));
			_progressSaveLoadService = progressSaveLoadDataService ??
				throw new ArgumentNullException(nameof(progressSaveLoadDataService));
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

			await _progressSaveLoadService.SaveToCloud(() => _isCanSave = true);
		}

		public void Dispose() =>
			Disable();
	}
}