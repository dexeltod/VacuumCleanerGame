using System;
using System.Collections.Generic;
using System.Linq;
using Sources.BusinessLogic.Repository;
using Sources.BusinessLogic.Services;
using Sources.Controllers.Common;
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
		private readonly AudioSource _audioSource;
		private readonly GameplayInterfaceSoundPlayer _gameplayInterfaceSoundPlayer;
		private readonly IPersistentProgressService _persistentProgressServiceProvider;
		private readonly IProgressEntityRepository _progressEntityRepository;
		private readonly ISaveLoader _saveLoader;

		private readonly IShopService _shopService;
		private readonly IEnumerable<IUpgradeElementChangeableView> _upgradeElementChangeableViews;
		private IReadOnlyList<IStatUpgradeEntityReadOnly> _entities;

		public UpgradeElementPresenter(
			IEnumerable<IUpgradeElementChangeableView> panel,
			IPersistentProgressService persistentProgressServiceProvider,
			IProgressEntityRepository progressEntityRepository,
			ISaveLoader saveLoader,
			AudioClip soundBuy,
			AudioClip soundClose,
			AudioSource audioSource,
			IShopService shopService
		)
		{
			_upgradeElementChangeableViews = panel ?? throw new ArgumentNullException(nameof(panel));
			_persistentProgressServiceProvider = persistentProgressServiceProvider
			                                     ?? throw new ArgumentNullException(nameof(persistentProgressServiceProvider));
			_progressEntityRepository =
				progressEntityRepository ?? throw new ArgumentNullException(nameof(progressEntityRepository));
			_saveLoader = saveLoader ?? throw new ArgumentNullException(nameof(saveLoader));
			_audioSource = audioSource ?? throw new ArgumentNullException(nameof(audioSource));

			_shopService = shopService ?? throw new ArgumentNullException(nameof(shopService));

			_gameplayInterfaceSoundPlayer = new GameplayInterfaceSoundPlayer(soundBuy, soundClose, audioSource);
		}

		private int Money =>
			_persistentProgressServiceProvider
				.GlobalProgress.ResourceModel.SoftCurrency.ReadOnlyValue;

		public async void Upgrade(int id)
		{
			if (_shopService.TryAddOneProgressPoint(id) == false)
				throw new InvalidOperationException("Failed to add progress point");

			_gameplayInterfaceSoundPlayer.PlayBuySound();

			await _saveLoader.Save(_persistentProgressServiceProvider.GlobalProgress);
			SetView(id);
		}

		public override void Enable()
		{
			_entities = _progressEntityRepository.GetEntities();

			foreach (IUpgradeElementChangeableView value in _upgradeElementChangeableViews)
				value.BuyButtonPressed += Upgrade;

			foreach (IStatUpgradeEntityReadOnly entity in _entities)
				entity.LevelProgress.Changed += LevelProgressChanged;
		}

		public override void Disable()
		{
			foreach (IUpgradeElementChangeableView value in _upgradeElementChangeableViews)
				value.BuyButtonPressed -= Upgrade;

			_entities = _progressEntityRepository.GetEntities();

			foreach (IStatUpgradeEntityReadOnly entity in _entities)
				entity.LevelProgress.Changed -= LevelProgressChanged;
		}

		private void LevelProgressChanged()
		{
		}

		private void SetView(int id)
		{
			IUpgradeElementChangeableView panel = _upgradeElementChangeableViews.ElementAt(id);

			panel.AddProgressPointColor();
			panel.SetPriceText(_progressEntityRepository.GetPrice(id));
		}
	}
}
