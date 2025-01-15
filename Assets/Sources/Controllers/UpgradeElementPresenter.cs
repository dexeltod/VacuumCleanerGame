using System;
using System.Collections.Generic;
using Sources.BuisenessLogic.Repository;
using Sources.Controllers.Common;
using Sources.Controllers.Shop;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.DomainInterfaces.Models.Shop.Upgrades;
using Sources.PresentationInterfaces;
using UnityEngine;

namespace Sources.Controllers
{
	public class UpgradeElementPresenter : Presenter, IUpgradeElementPresenter
	{
		private readonly Dictionary<int, IUpgradeElementChangeableView> _upgradeElementChangeableViews;
		private readonly IPersistentProgressService _persistentProgressServiceProvider;
		private readonly IUpgradeWindowPresenter _upgradeWindowPresenter;
		private readonly IGameplayInterfacePresenter _gameplayInterfacePresenterProvider;
		private readonly IProgressEntityRepository _progressEntityRepository;
		private readonly ISaveLoader _saveLoader;
		private readonly AudioSource _audioSource;

		private readonly ShopPurchaseController _shopPurchaseController;
		private IReadOnlyList<IStatUpgradeEntityReadOnly> _entities;
		private readonly GameplayInterfaceSoundPlayer _gameplayInterfaceSoundPlayer;

		public UpgradeElementPresenter(
			Dictionary<int, IUpgradeElementChangeableView> panel,
			IPersistentProgressService persistentProgressServiceProvider,
			IUpgradeWindowPresenter upgradeWindowPresenter,
			IGameplayInterfacePresenter gameplayInterfacePresenterProvider,
			IProgressEntityRepository progressEntityRepository,
			ISaveLoader saveLoader,
			IResourcesProgressPresenter resourcesProgressPresenterProvider,
			IPlayerModelRepository playerModelRepositoryProvider,
			AudioClip soundBuy,
			AudioClip soundClose,
			AudioSource audioSource
		)
		{
			_upgradeElementChangeableViews = panel ?? throw new ArgumentNullException(nameof(panel));
			_persistentProgressServiceProvider = persistentProgressServiceProvider ??
			                                     throw new ArgumentNullException(nameof(persistentProgressServiceProvider));
			_upgradeWindowPresenter = upgradeWindowPresenter ??
			                          throw new ArgumentNullException(nameof(upgradeWindowPresenter));
			_gameplayInterfacePresenterProvider = gameplayInterfacePresenterProvider ??
			                                      throw new ArgumentNullException(nameof(gameplayInterfacePresenterProvider));
			_progressEntityRepository = progressEntityRepository ??
			                            throw new ArgumentNullException(nameof(progressEntityRepository));
			_saveLoader = saveLoader ?? throw new ArgumentNullException(nameof(saveLoader));
			_audioSource = audioSource ?? throw new ArgumentNullException(nameof(audioSource));

			_shopPurchaseController = new ShopPurchaseController(
				_progressEntityRepository,
				resourcesProgressPresenterProvider,
				playerModelRepositoryProvider
			);

			_gameplayInterfaceSoundPlayer = new GameplayInterfaceSoundPlayer(soundBuy, soundClose, audioSource);
		}

		private int Money =>
			_persistentProgressServiceProvider
				.GlobalProgress.ResourceModelReadOnly.SoftCurrency.Value;

		public async void Upgrade(int id)
		{
			if (_shopPurchaseController.TryAddOneProgressPoint(id) == false)
				throw new InvalidOperationException("Failed to add progress point");

			_gameplayInterfaceSoundPlayer.PlayBuySound();

			await _saveLoader.Save(_persistentProgressServiceProvider.GlobalProgress);
			SetView(id);
		}

		private void SetView(int id)
		{
			IUpgradeElementChangeableView panel = _upgradeElementChangeableViews[id];

			panel.AddProgressPointColor();
			panel.SetPriceText(_progressEntityRepository.GetPrice(id));

			_upgradeWindowPresenter.SetMoney(Money);
			_gameplayInterfacePresenterProvider.SetSoftCurrency(Money);
		}

		public override void Enable()
		{
			_entities = _progressEntityRepository.GetEntities();

			foreach (IStatUpgradeEntityReadOnly entity in _entities)
				entity.LevelProgress.Changed += LevelProgressChanged;
		}

		public override void Disable()
		{
			_entities = _progressEntityRepository.GetEntities();

			foreach (IStatUpgradeEntityReadOnly entity in _entities)
				entity.LevelProgress.Changed -= LevelProgressChanged;
		}

		private void LevelProgressChanged()
		{
		}
	}
}