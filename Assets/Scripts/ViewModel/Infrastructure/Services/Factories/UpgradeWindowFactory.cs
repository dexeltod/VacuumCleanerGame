using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Model;
using Model.Configs;
using Model.DI;
using Model.Infrastructure.Data;
using UnityEngine;
using View.SceneEntity;
using View.UI.Shop;

namespace ViewModel.Infrastructure.Services.Factories
{
	public class UpgradeWindowFactory : IUpgradeWindowFactory
	{
		private readonly IAssetProvider _assetProvider;
		private readonly ISaveLoadDataService _progress;

		private bool _isInitialized;
		private ShopElementFactory _shopElementFactory;
		private GameObject _upgradeWindow;

		private List<UpgradeElementView> _buttonElements;
		private List<string> _buttonNames = new();

		public UpgradeWindow UpgradeWindow { get; private set; }

		public UpgradeWindowFactory()
		{
			_isInitialized = false;
			_progress = ServiceLocator.Container.GetSingle<ISaveLoadDataService>();
			_assetProvider = ServiceLocator.Container.GetSingle<IAssetProvider>();
		}

		public UpgradeWindow GetUpgradeWindow()
		{
			if (_upgradeWindow != null)
				return _upgradeWindow.GetComponent<UpgradeWindow>();

			throw new InvalidOperationException();
		}

		public async UniTask<GameObject> Create()
		{
			if (_isInitialized)
				throw new InvalidOperationException();

			await InstantiateWindow();
			ConstructWindow();
			await InitButtons();

			new ShopPurchaseController(UpgradeWindow, _buttonElements, _buttonNames);

			_isInitialized = true;
			return _upgradeWindow;
		}

		private async Task InitButtons()
		{
			_buttonElements = await _shopElementFactory.InstantiateElements(UpgradeWindow.UpgradeElementsTransform);
			_buttonNames = _buttonElements.Select(x => x.Title).ToList();
		}

		private void ConstructWindow()
		{
			UpgradeWindow = _upgradeWindow.GetComponent<UpgradeWindow>();
			UpgradeWindow.Construct();
		}

		private async Task InstantiateWindow()
		{
			ShopProgress shopProgress = await LoadUpgradeDataAsync(_progress);
			_shopElementFactory ??= new ShopElementFactory(shopProgress);
			_upgradeWindow = await _assetProvider.Instantiate(ConstantNames.UIElementNames.UpgradeWindow);
		}

		private async Task<ShopProgress> LoadUpgradeDataAsync(ISaveLoadDataService saveLoadDataService)
		{
			GameProgressModel result = await saveLoadDataService.LoadProgress();
			return result.ShopProgress;
		}
	}
}