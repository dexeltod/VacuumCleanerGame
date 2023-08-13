using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sources.Application.Utils.Configs;
using Sources.DIService;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories;
using Sources.InfrastructureInterfaces;
using Sources.PresetrationInterfaces;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.UI;
using Sources.View.UI.Shop;
using UnityEngine;

namespace Sources.Application.UI
{
	public class UpgradeWindowFactory : IUpgradeWindowFactory
	{
		private readonly IShopItemFactory _shopItemFactory;
		private readonly IResourceProvider _assetProvider;
		private readonly IGameProgressModel _progress;

		private bool _isInitialized;
		private ShopElementFactory _shopElementFactory;
		private GameObject _upgradeWindow;

		private List<IUpgradeElementView> _buttonElements;

		public IUpgradeWindow UpgradeWindow { get; private set; }

		public UpgradeWindowFactory(IShopItemFactory shopItemFactory)
		{
			_shopItemFactory = shopItemFactory;

			_isInitialized = false;
			_progress = GameServices.Container.Get<IPersistentProgressService>().GameProgress;
			_assetProvider = GameServices.Container.Get<IResourceProvider>();
		}

		public GameObject Create()
		{
			if (_isInitialized)
				throw new InvalidOperationException("GameObject is initialized");

			InstantiateWindow();
			ConstructWindow();
			InitButtons();

			IUpgradeItemList items = _shopItemFactory.LoadItems();
			
			new ShopPurchaseController(items.Items, UpgradeWindow, _buttonElements);

			_isInitialized = true;
			return _upgradeWindow;
		}

		private void InstantiateWindow()
		{
			_shopElementFactory ??= new ShopElementFactory(_progress.ShopProgress);
			_upgradeWindow =  _assetProvider.Instantiate(ResourcesAssetPath.Scene.UI.UpgradeWindow);
		}

		private void InitButtons() => 
			_buttonElements = _shopElementFactory.InstantiateElements(UpgradeWindow.ContainerTransform);

		private void ConstructWindow()
		{
			UpgradeWindow = _upgradeWindow.GetComponent<UpgradeWindow>();
			UpgradeWindow.Construct();
		}
	}
}