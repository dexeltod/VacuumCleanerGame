using System;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces.Providers;
using Sources.PresentationInterfaces;

namespace Sources.Controllers
{
	public class UpgradeWindowPresenter : Presenter, IDisposable, IUpgradeWindowPresenter
	{
		private readonly IUpgradeWindowPresentation _upgradeWindowPresentation;
		private readonly IProgressSaveLoadDataService _progressSaveLoadService;
		private readonly IGameplayInterfacePresenter _gameplayInterfacePresenter;
		private readonly IResourcesProgressPresenterProvider _resourcesProgressPresenterProvider;

		private bool _isCanSave;

		public UpgradeWindowPresenter(
			IUpgradeWindowPresentation upgradeWindowPresentation,
			IProgressSaveLoadDataService progressSaveLoadDataService,
			IGameplayInterfacePresenter gameplayInterfacePresenter,
			IResourcesProgressPresenterProvider resourcesProgressPresenterProvider
		)
		{
			_upgradeWindowPresentation = upgradeWindowPresentation ??
				throw new ArgumentNullException(nameof(upgradeWindowPresentation));
			_progressSaveLoadService = progressSaveLoadDataService ??
				throw new ArgumentNullException(nameof(progressSaveLoadDataService));
			_gameplayInterfacePresenter = gameplayInterfacePresenter ??
				throw new ArgumentNullException(nameof(gameplayInterfacePresenter));
			_resourcesProgressPresenterProvider = resourcesProgressPresenterProvider ??
				throw new ArgumentNullException(nameof(resourcesProgressPresenterProvider));
		}

		private int SoftCurrencyCount => _resourcesProgressPresenterProvider.Implementation.SoftCurrency.Count;

		public void Dispose() =>
			Disable();

		public override void Enable()
		{
			_upgradeWindowPresentation.SetMoney(SoftCurrencyCount);

			_gameplayInterfacePresenter.Disable();
			_upgradeWindowPresentation.CloseMenuButton.onClick.AddListener(OnClose);
		}

		public override void Disable()
		{
			_upgradeWindowPresentation.CloseMenuButton.onClick.RemoveListener(OnClose);

			_gameplayInterfacePresenter.Enable();
		}

		public void SetMoney(int money) =>
			_upgradeWindowPresentation.SetMoney(money);

		public void EnableWindow()
		{
			_upgradeWindowPresentation.SetMoney(SoftCurrencyCount);
			_upgradeWindowPresentation.UpgradeWindowMain.SetActive(true);
		}

		private async void OnClose()
		{
			_upgradeWindowPresentation.UpgradeWindowMain.SetActive(false);

			if (_isCanSave == false)
				return;

			_isCanSave = false;

			await _progressSaveLoadService.SaveToCloud(() => _isCanSave = true);
		}
	}
}