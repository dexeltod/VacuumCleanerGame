using System.Collections.Generic;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Factories;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Upgrade;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.UI;
using Sources.Utils.Configs;
using Sources.View.UI.Shop;
using UnityEngine;

namespace Sources.Application.UI
{
	public class UpgradeWindowFactory : IUpgradeWindowFactory
	{
		private readonly IResourcesProgressPresenter _resourceProgressPresenter;
		private readonly IShopItemFactory            _shopItemFactory;
		private readonly IAssetProvider              _assetProvider;
		private readonly IGameProgressModel          _progress;

		private ShopElementFactory         _shopElementFactory;
		private List<UpgradeElementPrefab> _upgradeElementsPrefabs;

		private GameObject     _upgradeWindow;
		public  IUpgradeWindow UpgradeWindow { get; private set; }

		public UpgradeWindowFactory
		(
			IAssetProvider              assetProvider,
			IShopItemFactory            shopItemFactory,
			IResourcesProgressPresenter resourceProgressPresenter,
			IGameProgressModel          progress
		)
		{
			_assetProvider             = assetProvider;
			_shopItemFactory           = shopItemFactory;
			_resourceProgressPresenter = resourceProgressPresenter;
			_progress                  = progress;
		}

		public GameObject Create()
		{
			if (_upgradeWindow != null)
				return _upgradeWindow;

			Initialize();

			IUpgradeItemData[] items = _shopItemFactory.LoadItems();

			ShopPurchaseController shopPurchaseController = new ShopPurchaseController
			(
				UpgradeWindow,
				_upgradeElementsPrefabs
			);

			return _upgradeWindow;
		}

		private void Initialize()
		{
			InstantiateWindow();
			ConstructWindow();
			InitButtons();
		}

		private void InstantiateWindow()
		{
			_shopElementFactory ??= new ShopElementFactory(_progress.ShopProgress);
			_upgradeWindow      =   _assetProvider.Instantiate(ResourcesAssetPath.Scene.UIResources.UpgradeWindow);
		}

		private void InitButtons() =>
			_upgradeElementsPrefabs = _shopElementFactory
				.InstantiateElementPrefabs(UpgradeWindow.ContainerTransform);

		private void ConstructWindow()
		{
			IResource<int> resource = _resourceProgressPresenter.SoftCurrency;

			UpgradeWindow = _upgradeWindow.GetComponent<UpgradeWindow>();
			UpgradeWindow.Construct(resource);
		}
	}
}