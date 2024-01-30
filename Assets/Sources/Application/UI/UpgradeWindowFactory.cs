using System;
using System.Collections.Generic;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Factories;
using Sources.InfrastructureInterfaces.DTO;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Upgrade;
using Sources.Presentation.UI.Shop;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.UI;
using Sources.Utils.Configs;
using Sources.Utils.Configs.Scripts;
using UnityEngine;

namespace Sources.Application.UI
{
	public class UpgradeWindowFactory : IUpgradeWindowFactory
	{
		private readonly IResourcesProgressPresenter _resourceProgressPresenter;
		private readonly IProgressUpgradeFactory _progressUpgradeFactory;
		private readonly IAssetProvider _assetProvider;
		private readonly IShopProgressProvider _shopProgressProvider;
		private readonly IPlayerProgressProvider _playerProgressProvider;
		private readonly ITranslatorService _translatorService;
		private readonly IPersistentProgressService _progress;

		private ShopElementFactory _shopElementFactory;
		private List<UpgradeElementPrefabView> _upgradeElementsPrefabs;

		private GameObject _upgradeWindow;
		public IUpgradeWindow UpgradeWindow { get; private set; }
		private IGameProgress ShopProgress => _progress.GameProgress.ShopProgress;
		private string UpgradeWindowPath => ResourcesAssetPath.Scene.UIResources.UpgradeWindow;

		public UpgradeWindowFactory(
			IAssetProvider assetProvider,
			IProgressUpgradeFactory progressUpgradeFactory,
			IResourcesProgressPresenter resourceProgressPresenter,
			IPersistentProgressService progress,
			IShopProgressProvider shopProgressProvider,
			IPlayerProgressProvider playerProgressProvider,
			ITranslatorService translatorService
		)
		{
			_assetProvider = assetProvider ?? throw new ArgumentNullException(nameof(assetProvider));
			_progressUpgradeFactory = progressUpgradeFactory ??
				throw new ArgumentNullException(nameof(progressUpgradeFactory));
			_resourceProgressPresenter = resourceProgressPresenter ??
				throw new ArgumentNullException(nameof(resourceProgressPresenter));
			_shopProgressProvider
				= shopProgressProvider ?? throw new ArgumentNullException(nameof(shopProgressProvider));
			_playerProgressProvider = playerProgressProvider ??
				throw new ArgumentNullException(nameof(playerProgressProvider));
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));

			_progress = progress ?? throw new ArgumentNullException(nameof(progress));
		}

		public IUpgradeWindow Create()
		{
			if (UpgradeWindow != null)
				return UpgradeWindow;

			ShopElementFactory elementFactory = new ShopElementFactory(
				ShopProgress,
				_assetProvider,
				_translatorService
			);

			_upgradeWindow = _assetProvider.Instantiate(UpgradeWindowPath);
			UpgradeWindow = _upgradeWindow.GetComponent<IUpgradeWindow>();
			InitButtons(elementFactory);

			IUpgradeItemData[] items = _progressUpgradeFactory.LoadItems();

			ShopPurchaseController shopPurchaseController = new ShopPurchaseController(
				UpgradeWindow,
				_upgradeElementsPrefabs,
				_resourceProgressPresenter,
				_shopProgressProvider,
				_playerProgressProvider
			);

			return UpgradeWindow;
		}

		private void InitButtons(ShopElementFactory shopElementFactory) =>
			_upgradeElementsPrefabs = shopElementFactory.Instantiate(UpgradeWindow.ContainerTransform);
	}
}