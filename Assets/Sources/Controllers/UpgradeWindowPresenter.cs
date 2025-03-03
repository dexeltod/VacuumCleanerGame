using System;
using System.Collections.Generic;
using Sources.BusinessLogic.Repository;
using Sources.BusinessLogic.Services;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.DomainInterfaces.Models.Shop.Upgrades;
using Sources.DomainInterfaces.ViewEntities;
using Sources.PresentationInterfaces;
using Sources.PresentationInterfaces.Common;

namespace Sources.Controllers
{
	public class UpgradeWindowPresenter : Presenter, IDisposable, IUpgradeWindowPresenter
	{
		private readonly IReadOnlyList<IStatUpgradeEntityReadOnly> _entities;
		private readonly IView _gameplayInterface;
		private readonly IPersistentProgressService _persistentProgressServiceProvider;

		private readonly Dictionary<IStatUpgradeEntityReadOnly, Action> _progressChangeHandlers = new();
		private readonly IProgressEntityRepository _progressEntityRepository;
		private readonly IProgressSaveLoadDataService _progressSaveLoadService;
		private readonly ISaveLoader _saveLoader;
		private readonly IShopService _shopService;
		private readonly IReadOnlyProgress<int> _softCurrency;
		private readonly Dictionary<int, IUpgradeElementPrefabView> _upgradeElementPrefab;
		private readonly IViewEntity _upgradeWindowEntity;
		private readonly IUpgradeWindowPresentation _upgradeWindowPresentation;

		private bool _isCanSave;

		public UpgradeWindowPresenter(
			IUpgradeWindowPresentation upgradeWindowPresentation,
			IView gameplayInterfaceStatus,
			IReadOnlyProgress<int> softCurrency,
			Dictionary<int, IUpgradeElementPrefabView> upgradeElementChangeableViews,
			IPersistentProgressService persistentProgressServiceProvider,
			IProgressEntityRepository progressEntityRepository,
			ISaveLoader saveLoader,
			IShopService shopService,
			IReadOnlyList<IStatUpgradeEntityReadOnly> entities,
			IViewEntity upgradeWindowEntity) : base(upgradeWindowEntity)
		{
			_upgradeWindowPresentation =
				upgradeWindowPresentation ?? throw new ArgumentNullException(nameof(upgradeWindowPresentation));
			_gameplayInterface = gameplayInterfaceStatus ?? throw new ArgumentNullException(nameof(gameplayInterfaceStatus));
			_softCurrency = softCurrency ?? throw new ArgumentNullException(nameof(softCurrency));
			_upgradeElementPrefab = upgradeElementChangeableViews
			                        ?? throw new ArgumentNullException(nameof(upgradeElementChangeableViews));
			_persistentProgressServiceProvider = persistentProgressServiceProvider
			                                     ?? throw new ArgumentNullException(nameof(persistentProgressServiceProvider));
			_progressEntityRepository =
				progressEntityRepository ?? throw new ArgumentNullException(nameof(progressEntityRepository));
			_saveLoader = saveLoader ?? throw new ArgumentNullException(nameof(saveLoader));
			_shopService = shopService ?? throw new ArgumentNullException(nameof(shopService));
			_entities = entities ?? throw new ArgumentNullException(nameof(entities));
			_upgradeWindowEntity = upgradeWindowEntity ?? throw new ArgumentNullException(nameof(upgradeWindowEntity));
		}

		private int SoftCurrencyReadOnlyValue => _softCurrency.ReadOnlyValue;

		public void Dispose() => Disable();

		public override void Enable()
		{
			base.Enable();
			_upgradeWindowPresentation.Enable();
			_upgradeWindowPresentation.UpgradeWindowMain.SetActive(true);

			Subscribe();

			_upgradeWindowPresentation.SetMoney(SoftCurrencyReadOnlyValue);
		}

		public override void Disable()
		{
			base.Disable();
			Unsubscribe();
			_upgradeWindowPresentation.UpgradeWindowMain.SetActive(false);
		}

		private void LevelProgressChanged(IStatUpgradeEntityReadOnly entityReadOnly)
		{
			SetView(entityReadOnly.ConfigId);
		}

		private void OnClose()
		{
			_upgradeWindowPresentation.AudioClose.Play();

			_upgradeWindowEntity.SetActive(false);
			Disable();
		}

		private void OnSoftCurrencyChanged() => _upgradeWindowPresentation.SetMoney(SoftCurrencyReadOnlyValue);

		private void SetView(int id)
		{
			IUpgradeElementChangeableView panel =
				_upgradeElementPrefab[id]
				?? throw new ArgumentNullException($"UpgradeElementChangeableView with id {id} not found");

			panel.AddProgressPointColor();
			panel.SetPriceText(_progressEntityRepository.GetPrice(id));
		}

		private void Subscribe()
		{
			SubscribeUpgradeElementViews();

			SubscribeOnProgressChange();

			_softCurrency.Changed += OnSoftCurrencyChanged;

			_upgradeWindowPresentation.CloseMenuButton.onClick.AddListener(OnClose);
		}

		private void SubscribeOnProgressChange()
		{
			foreach (IStatUpgradeEntityReadOnly entity in _entities)
			{
				Action handler = () => LevelProgressChanged(entity);
				_progressChangeHandlers[entity] = handler;

				entity.LevelProgress.Changed += handler;
			}
		}

		private void SubscribeUpgradeElementViews()
		{
			foreach (IUpgradeElementPrefabView value in _upgradeElementPrefab.Values)
				value.BuyButtonPressed += Upgrade;
		}

		private void Unsubscribe()
		{
			foreach (IUpgradeElementPrefabView value in _upgradeElementPrefab.Values)
				value.BuyButtonPressed -= Upgrade;

			UnsubscribeOnProgressChange();

			_softCurrency.Changed -= OnSoftCurrencyChanged;
			_upgradeWindowPresentation.CloseMenuButton.onClick.RemoveListener(OnClose);
		}

		private void UnsubscribeOnProgressChange()
		{
			foreach (IStatUpgradeEntityReadOnly entity in _entities)
				if (_progressChangeHandlers.TryGetValue(entity, out Action handler))
					entity.LevelProgress.Changed -= handler;

			_progressChangeHandlers.Clear();
		}

		private async void Upgrade(int id)
		{
			if (_shopService.TryAddOneProgressPoint(id) == false)
				throw new InvalidOperationException("Failed to add progress point");

			await _saveLoader.Save(_persistentProgressServiceProvider.GlobalProgress);
		}
	}
}
