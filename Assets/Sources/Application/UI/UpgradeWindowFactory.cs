using System.Collections.Generic;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Factories;
using Sources.InfrastructureInterfaces.DTO;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Upgrade;
using Sources.Presentation.UI.Shop;
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
		private readonly IPersistentProgressService _progress;

		private ShopElementFactory _shopElementFactory;
		private List<UpgradeElementPrefabView> _upgradeElementsPrefabs;

		private GameObject _upgradeWindow;
		public IUpgradeWindow UpgradeWindow { get; private set; }
		private IGameProgress ShopProgress => _progress.GameProgress.ShopProgress;

		public UpgradeWindowFactory(
			IAssetProvider assetProvider,
			IProgressUpgradeFactory progressUpgradeFactory,
			IResourcesProgressPresenter resourceProgressPresenter,
			IPersistentProgressService progress,
			IShopProgressProvider shopProgressProvider,
			IPlayerProgressProvider playerProgressProvider
		)
		{
			_assetProvider = assetProvider;
			_progressUpgradeFactory = progressUpgradeFactory;
			_resourceProgressPresenter = resourceProgressPresenter;
			_shopProgressProvider = shopProgressProvider;
			_playerProgressProvider = playerProgressProvider;

			_progress = progress;
		}

		public GameObject Create()
		{
			if (_upgradeWindow != null)
				return _upgradeWindow;

			Initialize();

			IUpgradeItemData[] items = _progressUpgradeFactory.LoadItems();

			ShopPurchaseController shopPurchaseController = new ShopPurchaseController(
				UpgradeWindow,
				_upgradeElementsPrefabs,
				_resourceProgressPresenter,
				_shopProgressProvider,
				_playerProgressProvider
			);

			return _upgradeWindow;
		}

		private void Initialize()
		{
			InstantiateWindow();
			InitButtons();
		}

		private void InstantiateWindow()
		{
			_shopElementFactory ??= new ShopElementFactory(ShopProgress, _assetProvider);
			_upgradeWindow = _assetProvider.Instantiate(ResourcesAssetPath.Scene.UIResources.UpgradeWindow);
		}

		private void InitButtons() =>
			_upgradeElementsPrefabs = _shopElementFactory
				.Instantiate(UpgradeWindow.ContainerTransform);
	}
}