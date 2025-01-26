using System;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.PresentationInterfaces;
using Sources.PresentationInterfaces.Common;

namespace Sources.Controllers
{
	public class UpgradeWindowPresenter : Presenter, IDisposable, IUpgradeWindowPresenter
	{
		private readonly IView _gameplayInterfaceStatus;
		private readonly IProgressSaveLoadDataService _progressSaveLoadService;
		private readonly IReadOnlyProgress<int> _softCurrency;
		private readonly IUpgradeWindowPresentation _upgradeWindowPresentation;

		private bool _isCanSave;

		public UpgradeWindowPresenter(
			IUpgradeWindowPresentation upgradeWindowPresentation,
			IProgressSaveLoadDataService progressSaveLoadDataService,
			IView gameplayInterfaceStatus,
			IReadOnlyProgress<int> softCurrency)
		{
			_upgradeWindowPresentation =
				upgradeWindowPresentation ?? throw new ArgumentNullException(nameof(upgradeWindowPresentation));
			_progressSaveLoadService = progressSaveLoadDataService
			                           ?? throw new ArgumentNullException(nameof(progressSaveLoadDataService));
			_gameplayInterfaceStatus = gameplayInterfaceStatus ?? throw new ArgumentNullException(nameof(gameplayInterfaceStatus));
			_softCurrency = softCurrency ?? throw new ArgumentNullException(nameof(softCurrency));
		}

		private int SoftCurrencyReadOnlyValue => _softCurrency.ReadOnlyValue;

		public void Dispose() => Disable();

		public override void Enable()
		{
			_softCurrency.Changed += OnSoftCurrencyChanged;

			_upgradeWindowPresentation.SetMoney(SoftCurrencyReadOnlyValue);

			_gameplayInterfaceStatus.Disable();

			_upgradeWindowPresentation.CloseMenuButton.onClick.AddListener(OnClose);
		}

		public override void Disable()
		{
			_softCurrency.Changed -= OnSoftCurrencyChanged;
			_upgradeWindowPresentation.CloseMenuButton.onClick.RemoveListener(OnClose);
			_gameplayInterfaceStatus.Enable();
		}

		public void SetMoney(int money) => _upgradeWindowPresentation.SetMoney(money);

		public void EnableWindow()
		{
			_upgradeWindowPresentation.SetMoney(SoftCurrencyReadOnlyValue);
			_upgradeWindowPresentation.UpgradeWindowMain.SetActive(true);
		}

		private async void OnClose()
		{
			_gameplayInterfaceStatus.Enable();
			_upgradeWindowPresentation.AudioSource.Play();
			_upgradeWindowPresentation.UpgradeWindowMain.SetActive(false);

			if (_isCanSave == false)
				return;

			_isCanSave = false;

			await _progressSaveLoadService.SaveToCloud(() => _isCanSave = true);
			Disable();
		}

		private void OnSoftCurrencyChanged() => _upgradeWindowPresentation.SetMoney(SoftCurrencyReadOnlyValue);
	}
}
