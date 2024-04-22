using System;
using System.Collections.Generic;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Repositories;
using Sources.InfrastructureInterfaces.Providers;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.Upgrade;
using Sources.Utils;

namespace Sources.Controllers
{
	public class UpgradeElementPresenter : Presenter, IUpgradeElementPresenter
	{
		private readonly IProgressSetterFacade _progressSetterFacade;
		private readonly Dictionary<int, IUpgradeElementChangeableView> _upgradeElementChangeableViews;
		private readonly IPersistentProgressServiceProvider _persistentProgressServiceProvider;
		private readonly IUpgradeWindowPresenterProvider _upgradeWindowPresenter;
		private readonly IGameplayInterfacePresenterProvider _gameplayInterfacePresenterProvider;
		private readonly IUpgradeProgressRepository _upgradeProgressRepository;
		private readonly ISaveLoader _saveLoader;

		public UpgradeElementPresenter(
			IProgressSetterFacade progressSetterFacade,
			Dictionary<int, IUpgradeElementChangeableView> panel,
			IPersistentProgressServiceProvider persistentProgressServiceProvider,
			IUpgradeWindowPresenterProvider upgradeWindowPresenter,
			IGameplayInterfacePresenterProvider gameplayInterfacePresenterProvider,
			IUpgradeProgressRepository upgradeProgressRepository
		)
		{
			_progressSetterFacade = progressSetterFacade ??
				throw new ArgumentNullException(nameof(progressSetterFacade));
			_upgradeElementChangeableViews = panel ?? throw new ArgumentNullException(nameof(panel));

			_persistentProgressServiceProvider = persistentProgressServiceProvider ??
				throw new ArgumentNullException(nameof(persistentProgressServiceProvider));
			_upgradeWindowPresenter = upgradeWindowPresenter ??
				throw new ArgumentNullException(nameof(upgradeWindowPresenter));
			_gameplayInterfacePresenterProvider = gameplayInterfacePresenterProvider ??
				throw new ArgumentNullException(nameof(gameplayInterfacePresenterProvider));
			_upgradeProgressRepository = upgradeProgressRepository ??
				throw new ArgumentNullException(nameof(upgradeProgressRepository));
		}

		private IUpgradeWindowPresenter UpgradeWindowPresenter => _upgradeWindowPresenter.Implementation;

		private int Money =>
			_persistentProgressServiceProvider.Implementation
				.GlobalProgress.ResourceModelReadOnly.SoftCurrency.Count;

		public void Upgrade(int id)
		{
			_upgradeProgressRepository.GetEntity(id).AddOneLevel();
			int price = _upgradeProgressRepository.GetPrice(id);

			IUpgradeElementChangeableView panel = _upgradeElementChangeableViews[id];

			if (_progressSetterFacade.TryAddOneProgressPoint(id) == false)
				throw new InvalidOperationException("Failed to add progress point");

			panel.AddProgressPointColor();
			panel.SetPriceText(price);

			UpgradeWindowPresenter.SetMoney(Money);
			_gameplayInterfacePresenterProvider.Implementation.SetSoftCurrency(Money);
		}
	}
}