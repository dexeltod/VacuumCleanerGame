using System.Collections.Generic;
using Sources.Application.Utils;
using Sources.Application.Utils.Configs;
using Sources.DIService;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Factories;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Upgrade;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.UI;
using Sources.View.UI.Shop;
using UnityEngine;

namespace Sources.Application.UI
{
	public class UpgradeWindowFactory : IUpgradeWindowFactory
	{
		private readonly IShopItemFactory _shopItemFactory;
		private readonly IAssetProvider _assetProvider;
		private readonly IGameProgressModel _progress;
		private readonly IResourceService _resourceService;

		private ShopElementFactory _shopElementFactory;
		private List<UpgradeElementPrefab> _upgradeElementsPrefabs;

		private GameObject _upgradeWindow;
		public IUpgradeWindow UpgradeWindow { get; private set; }

		public UpgradeWindowFactory(IShopItemFactory shopItemFactory)
		{
			_shopItemFactory = shopItemFactory;

			_progress = GameServices.Container.Get<IPersistentProgressService>().GameProgress;
			_assetProvider = GameServices.Container.Get<IAssetProvider>();
			_resourceService = GameServices.Container.Get<IResourceService>();
		}

		public GameObject Create()
		{
			if (_upgradeWindow != null)
				return _upgradeWindow;

			Initialize();

			IUpgradeItemData[] items = _shopItemFactory.LoadItems();

			ShopPurchaseController shopPurchaseController = new ShopPurchaseController
			(
				UpgradeWindow, _upgradeElementsPrefabs
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
			_upgradeWindow = _assetProvider.Instantiate(ResourcesAssetPath.Scene.UI.UpgradeWindow);
		}

		private void InitButtons() =>
			_upgradeElementsPrefabs = _shopElementFactory.InstantiateElementPrefabs(UpgradeWindow.ContainerTransform);

		private void ConstructWindow()
		{
			IResource<int> resource = _resourceService.GetResource<int>(ResourceType.Soft);

			UpgradeWindow = _upgradeWindow.GetComponent<UpgradeWindow>();
			UpgradeWindow.Construct(resource);
		}
	}
}