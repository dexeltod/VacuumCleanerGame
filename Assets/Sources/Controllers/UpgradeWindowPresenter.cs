using System;
using System.Collections.Generic;
using Sources.BusinessLogic.Repository;
using Sources.BusinessLogic.Services;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.DomainInterfaces.Models.Shop.Upgrades;
using Sources.PresentationInterfaces;
using Sources.PresentationInterfaces.Common;

namespace Sources.Controllers
{
	public class UpgradeWindowPresenter : Presenter, IDisposable, IUpgradeWindowPresenter
	{
		private readonly IReadOnlyList<IStatUpgradeEntityReadOnly> _entities;
		private readonly IView _gameplayInterfaceStatus;
		private readonly IPersistentProgressService _persistentProgressServiceProvider;

		private readonly Dictionary<IStatUpgradeEntityReadOnly, Action> _progressChangeHandlers = new();
		private readonly IProgressEntityRepository _progressEntityRepository;
		private readonly IProgressSaveLoadDataService _progressSaveLoadService;
		private readonly ISaveLoader _saveLoader;
		private readonly IShopService _shopService;
		private readonly IReadOnlyProgress<int> _softCurrency;
		private readonly Dictionary<int, IUpgradeElementPrefabView> _upgradeElementPrefab;
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
			IReadOnlyList<IStatUpgradeEntityReadOnly> entities)
		{
			_upgradeWindowPresentation =
				upgradeWindowPresentation ?? throw new ArgumentNullException(nameof(upgradeWindowPresentation));
			_gameplayInterfaceStatus = gameplayInterfaceStatus ?? throw new ArgumentNullException(nameof(gameplayInterfaceStatus));
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
		}

		private int SoftCurrencyReadOnlyValue => _softCurrency.ReadOnlyValue;

		public void Dispose() => Disable();

		public override void Enable()
		{
			Subscribe();

			_upgradeWindowPresentation.SetMoney(SoftCurrencyReadOnlyValue);

			_gameplayInterfaceStatus.Disable();
		}

		public override void Disable()
		{
			Unsubscribe();

			_gameplayInterfaceStatus.Enable();
		}

		public void SetMoney(int money) => _upgradeWindowPresentation.SetMoney(money);

		public void EnableWindow()
		{
			_upgradeWindowPresentation.SetMoney(SoftCurrencyReadOnlyValue);
			_upgradeWindowPresentation.UpgradeWindowMain.SetActive(true);
		}

		private void LevelProgressChanged(IStatUpgradeEntityReadOnly entityReadOnly)
		{
			SetView(entityReadOnly.ConfigId);
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

			_progressChangeHandlers.Clear();

			_softCurrency.Changed -= OnSoftCurrencyChanged;
			_upgradeWindowPresentation.CloseMenuButton.onClick.RemoveListener(OnClose);
		}

		private void UnsubscribeOnProgressChange()
		{
			foreach (IStatUpgradeEntityReadOnly entity in _entities)
				if (_progressChangeHandlers.TryGetValue(entity, out Action handler))
					entity.LevelProgress.Changed -= handler;
		}

		private async void Upgrade(int id)
		{
			if (_shopService.TryAddOneProgressPoint(id) == false)
				throw new InvalidOperationException("Failed to add progress point");

			await _saveLoader.Save(_persistentProgressServiceProvider.GlobalProgress);
		}
	}
}
