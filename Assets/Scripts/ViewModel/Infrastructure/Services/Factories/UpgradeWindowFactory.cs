using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Model;
using Model.Configs;
using Model.Data;
using Model.DI;
using UnityEngine;
using View.SceneEntity;
using View.UI.Shop;

namespace ViewModel.Infrastructure.Services.Factories
{
	public class UpgradeWindowFactory : IUpgradeWindowFactory
	{
		private readonly IAssetProvider _assetProvider;
		private readonly GameProgressModel _progress;

		private bool _isInitialized;
		private ShopElementFactory _shopElementFactory;
		private GameObject _upgradeWindow;

		private List<UpgradeElementView> _buttonElements;
		private List<string> _buttonNames = new();

		public UpgradeWindow UpgradeWindow { get; private set; }

		public UpgradeWindowFactory()
		{
			_isInitialized = false;
			_progress = ServiceLocator.Container.GetSingle<IPersistentProgressService>().GameProgress;
			_assetProvider = ServiceLocator.Container.GetSingle<IAssetProvider>();
		}

		public async UniTask<GameObject> Create()
		{
			if (_isInitialized)
				throw new InvalidOperationException();

			await InstantiateWindow();
			ConstructWindow();
			await InitButtons();

			var shopPurchaseController = new ShopPurchaseController(UpgradeWindow, _buttonElements, _buttonNames);

			_isInitialized = true;
			return _upgradeWindow;
		}

		private async Task InstantiateWindow()
		{
			_shopElementFactory ??= new ShopElementFactory(_progress.ShopProgress);
			_upgradeWindow = await _assetProvider.Instantiate(ConstantNames.UIElementNames.UpgradeWindow);
		}

		private async UniTask InitButtons()
		{
			_buttonElements = await _shopElementFactory.InstantiateElements(UpgradeWindow.UpgradeElementsTransform);
			_buttonNames = _buttonElements.Select(x => x.ItemData.GetProgressName()).ToList();
		}

		private void ConstructWindow()
		{
			UpgradeWindow = _upgradeWindow.GetComponent<UpgradeWindow>();
			UpgradeWindow.Construct();
		}
	}
}