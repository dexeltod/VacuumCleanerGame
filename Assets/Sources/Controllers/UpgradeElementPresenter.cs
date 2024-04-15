using System;
using System.Collections.Generic;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
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
		private readonly Dictionary<int, IUpgradeElementChangeableView> _button;
		private readonly Dictionary<int, IUpgradeItemData> _upgradeItemData;
		private readonly IPersistentProgressServiceProvider _persistentProgressServiceProvider;
		private readonly IUpgradeWindowPresenterProvider _upgradeWindowPresenter;
		private readonly IGameplayInterfacePresenterProvider _gameplayInterfacePresenterProvider;
		private readonly ISaveLoader _saveLoader;

		public UpgradeElementPresenter(
			IProgressSetterFacade progressSetterFacade,
			Dictionary<int, IUpgradeElementChangeableView> button,
			Dictionary<int, IUpgradeItemData> upgradeItemData,
			IPersistentProgressServiceProvider persistentProgressServiceProvider,
			IUpgradeWindowPresenterProvider upgradeWindowPresenter,
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
			_gameplayInterfacePresenterProvider = gameplayInterfacePresenterProvider ??
				throw new ArgumentNullException(nameof(gameplayInterfacePresenterProvider));
		}

		private IUpgradeWindowPresenter UpgradeWindowPresenter => _upgradeWindowPresenter.Implementation;

		private int Money =>
			_persistentProgressServiceProvider.Implementation
				.GlobalProgress.ResourcesModel.SoftCurrency.Count;

		public void Upgrade(int id)
		{
			IUpgradeItemData itemData = _upgradeItemData[id];
			IUpgradeElementChangeableView button = _button[id];

			if (_progressSetterFacade.TryAddOneProgressPoint(itemData.IdName, itemData) == false)
				throw new InvalidOperationException();

			button.AddProgressPointColor();
			button.SetPriceText(itemData.Price);

			UpgradeWindowPresenter.SetMoney(Money);
			_gameplayInterfacePresenterProvider.Implementation.SetSoftCurrency(Money);
		}
	}
}