using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sources.Core.DI;
using Sources.Core.Domain.Progress;
using Sources.Core.Utils.Configs;
using Sources.DomainServices.Interfaces;
using Sources.Infrastructure.Services.Interfaces;
using Sources.View.Interfaces;
using Sources.View.SceneEntity;
using Sources.View.Services.UI;
using Sources.View.UI.Shop;
using UnityEngine;

namespace Sources.Infrastructure.Factories
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

		public IUpgradeWindow UpgradeWindow { get; private set; }

		public UpgradeWindowFactory()
		{
			_isInitialized = false;
			_progress = ServiceLocator.Container.Get<IPersistentProgressService>().GameProgress;
			_assetProvider = ServiceLocator.Container.Get<IAssetProvider>();
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

		private async UniTask InstantiateWindow()
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