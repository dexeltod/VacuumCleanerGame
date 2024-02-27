using System;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.InfrastructureInterfaces.Providers;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.Upgrade;

namespace Sources.Controllers
{
	public class UpgradeElementPresenter : Presenter, IUpgradeElementPresenter
	{
		private readonly IProgressSetterFacade _progressSetterFacade;
		private readonly IUpgradeElementChangeable _button;
		private readonly IUpgradeItemData _upgradeItemData;
		private readonly IPersistentProgressServiceProvider _persistentProgressServiceProvider;
		private readonly IUpgradeWindowPresenterProvider _upgradeWindowPresenter;
		private readonly IResourcesProgressPresenterProvider _resourcesProgressPresenter;
		private readonly IGameplayInterfacePresenterProvider _gameplayInterfacePresenterProvider;
		private readonly ISaveLoader _saveLoader;

		public UpgradeElementPresenter(
			IProgressSetterFacade progressSetterFacade,
			IUpgradeElementChangeable button,
			IUpgradeItemData upgradeItemData,
			IPersistentProgressServiceProvider persistentProgressServiceProvider,
			IUpgradeWindowPresenterProvider upgradeWindowPresenter,
			IResourcesProgressPresenterProvider resourcesProgressPresenter,
			IGameplayInterfacePresenterProvider gameplayInterfacePresenterProvider
		)
		{
			_progressSetterFacade = progressSetterFacade ??
				throw new ArgumentNullException(nameof(progressSetterFacade));
			_button = button ?? throw new ArgumentNullException(nameof(button));
			_upgradeItemData = upgradeItemData ?? throw new ArgumentNullException(nameof(upgradeItemData));
			_persistentProgressServiceProvider = persistentProgressServiceProvider ??
				throw new ArgumentNullException(nameof(persistentProgressServiceProvider));
			_upgradeWindowPresenter = upgradeWindowPresenter ??
				throw new ArgumentNullException(nameof(upgradeWindowPresenter));
			_resourcesProgressPresenter = resourcesProgressPresenter ??
				throw new ArgumentNullException(nameof(resourcesProgressPresenter));
			_gameplayInterfacePresenterProvider = gameplayInterfacePresenterProvider ??
				throw new ArgumentNullException(nameof(gameplayInterfacePresenterProvider));
		}

		private IUpgradeWindowPresenter UpgradeWindowPresenter => _upgradeWindowPresenter.Implementation;

		private int Money =>
			_persistentProgressServiceProvider.Implementation
				.GlobalProgress.ResourcesModel.SoftCurrency.Count;

		public void Upgrade()
		{
			if (_progressSetterFacade.TryAddOneProgressPoint(_upgradeItemData.IdName, _upgradeItemData) == false)
				throw new InvalidOperationException();

			_button.AddProgressPointColor();
			_button.SetPriceText(_upgradeItemData.Price);

			UpgradeWindowPresenter.SetMoney(Money);
			_gameplayInterfacePresenterProvider.Implementation.SetSoftCurrency(Money);
		}
	}
}