using System.Collections.Generic;
using Sources.Application.Utils.Configs;
using Sources.DIService;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Factory;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.UI;
using Sources.View;
using Sources.View.UI.Shop;
using UnityEngine;

namespace Sources.Application.UI
{
	public class UpgradeWindowFactory : IUpgradeWindowFactory
	{
		private readonly IShopItemFactory _shopItemFactory;
		private readonly IResourceProvider _assetProvider;
		private readonly IGameProgressModel _progress;

		private ShopElementFactory _shopElementFactory;
		private List<UpgradeElementPrefab> _upgradeElementsPrefabs;

		private GameObject _upgradeWindow;
		public IUpgradeWindow UpgradeWindow { get; private set; }

		public UpgradeWindowFactory(IShopItemFactory shopItemFactory)
		{
			_shopItemFactory = shopItemFactory;

			_progress = GameServices.Container.Get<IPersistentProgressService>().GameProgress;
			_assetProvider = GameServices.Container.Get<IResourceProvider>();
		}

		public GameObject Create()
		{
			if (_upgradeWindow != null)
				return _upgradeWindow;

			InstantiateWindow();
			ConstructWindow();
			InitButtons();

			IUpgradeItemData[] items = _shopItemFactory.LoadItems();

			new ShopPurchaseController(UpgradeWindow, _upgradeElementsPrefabs);
			return _upgradeWindow;
		}

		private void InstantiateWindow()
		{
			_shopElementFactory ??= new ShopElementFactory(_progress.ShopProgress);
			_upgradeWindow = _assetProvider.Instantiate(ResourcesAssetPath.Scene.UI.UpgradeWindow);
		}

		private void InitButtons() =>
			_upgradeElementsPrefabs = _shopElementFactory.InstantiateElementPrefabs(UpgradeWindow.ContainerTransform);

		private void ConstructWindow()
		{
			UpgradeWindow = _upgradeWindow.GetComponent<UpgradeWindow>();
			UpgradeWindow.Construct();
		}
	}
}