using System;
using System.Collections.Generic;
using Sources.Controllers.Common;
using Sources.Controllers.Shop;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.DomainInterfaces.Models.Shop.Upgrades;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Providers;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;

namespace Sources.Controllers
{
	public class UpgradeElementPresenter : Presenter, IUpgradeElementPresenter
	{
		private readonly Dictionary<int, IUpgradeElementChangeableView> _upgradeElementChangeableViews;
		private readonly IPersistentProgressServiceProvider _persistentProgressServiceProvider;
		private readonly IUpgradeWindowPresenterProvider _upgradeWindowPresenter;
		private readonly IGameplayInterfacePresenterProvider _gameplayInterfacePresenterProvider;
		private readonly IProgressService _progressService;
		private readonly ISaveLoader _saveLoader;

		private readonly ShopPurchaseController _shopPurchaseController;
		private IReadOnlyList<IUpgradeEntityReadOnly> _entities;

		public UpgradeElementPresenter(
			Dictionary<int, IUpgradeElementChangeableView> panel,
			IPersistentProgressServiceProvider persistentProgressServiceProvider,
			IUpgradeWindowPresenterProvider upgradeWindowPresenter,
			IGameplayInterfacePresenterProvider gameplayInterfacePresenterProvider,
			IProgressService upgradeProgressRepository,
			ISaveLoader saveLoader,
			IResourcesProgressPresenterProvider resourcesProgressPresenterProvider,
			IPlayerModelRepositoryProvider playerModelRepositoryProvider
		)
		{
			_upgradeElementChangeableViews = panel ?? throw new ArgumentNullException(nameof(panel));
			_persistentProgressServiceProvider = persistentProgressServiceProvider ??
				throw new ArgumentNullException(nameof(persistentProgressServiceProvider));
			_upgradeWindowPresenter = upgradeWindowPresenter ??
				throw new ArgumentNullException(nameof(upgradeWindowPresenter));
			_gameplayInterfacePresenterProvider = gameplayInterfacePresenterProvider ??
				throw new ArgumentNullException(nameof(gameplayInterfacePresenterProvider));
			_progressService = upgradeProgressRepository ??
				throw new ArgumentNullException(nameof(upgradeProgressRepository));
			_saveLoader = saveLoader ?? throw new ArgumentNullException(nameof(saveLoader));

			_shopPurchaseController = new ShopPurchaseController(
				_progressService,
				resourcesProgressPresenterProvider.Self,
				playerModelRepositoryProvider.Self
			);
		}

		private IUpgradeWindowPresenter UpgradeWindowPresenter => _upgradeWindowPresenter.Self;

		private int Money =>
			_persistentProgressServiceProvider.Self
				.GlobalProgress.ResourceModelReadOnly.SoftCurrency.Value;

		public void Upgrade(int id)
		{
			if (_shopPurchaseController.TryAddOneProgressPoint(id) == false)
				throw new InvalidOperationException("Failed to add progress point");

			_saveLoader.Save(_persistentProgressServiceProvider.Self.GlobalProgress);
			SetView(id);
		}

		private void SetView(int id)
		{
			IUpgradeElementChangeableView panel = _upgradeElementChangeableViews[id];

			panel.AddProgressPointColor();
			panel.SetPriceText(_progressService.GetPrice(id));

			UpgradeWindowPresenter.SetMoney(Money);
			_gameplayInterfacePresenterProvider.Self.SetSoftCurrency(Money);
		}

		public override void Enable()
		{
			_entities = _progressService.GetEntities();

			foreach (IUpgradeEntityReadOnly entity in _entities)
				entity.CurrentLevel.Changed += OnLevelChanged;
		}

		public override void Disable()
		{
			_entities = _progressService.GetEntities();

			foreach (IUpgradeEntityReadOnly entity in _entities)
				entity.CurrentLevel.Changed -= OnLevelChanged;
		}

		private void OnLevelChanged()
		{
			// TODO: ЗАБЫЛ ЧЕ Я ТУТ ХОТЕЛ СДЕЛАТЬ
			throw new NotImplementedException();
		}
	}
}