using System;
using System.Collections.Generic;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using Sources.Domain.Temp;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Repositories;
using Sources.InfrastructureInterfaces.Providers;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;

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
		private IReadOnlyList<IUpgradeEntityReadOnly> _entities;

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
				.GlobalProgress.ResourceModelReadOnly.SoftCurrency.Value;

		public void Upgrade(int id)
		{
			_upgradeProgressRepository.AddOneLevel(id);

			if (_progressSetterFacade.TryAddOneProgressPoint(id) == false)
				throw new InvalidOperationException("Failed to add progress point");

			SetView(id);
		}

		private void SetView(int id)
		{
			IUpgradeElementChangeableView panel = _upgradeElementChangeableViews[id];

			panel.AddProgressPointColor();
			panel.SetPriceText(_upgradeProgressRepository.GetPrice(id));

			UpgradeWindowPresenter.SetMoney(Money);
			_gameplayInterfacePresenterProvider.Implementation.SetSoftCurrency(Money);
		}

		public override void Enable()
		{
			_entities = _upgradeProgressRepository.GetEntities();

			foreach (IUpgradeEntityReadOnly entity in _entities)
				entity.CurrentLevel.Changed += OnLevelChanged;
		}

		public override void Disable()
		{
			_entities = _upgradeProgressRepository.GetEntities();

			foreach (IUpgradeEntityReadOnly entity in _entities)
				entity.CurrentLevel.Changed -= OnLevelChanged;
		}

		private void OnLevelChanged()
		{
			throw new NotImplementedException();
		}
	}
}