using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Model.DI;
using Model.ScriptableObjects.UpgradeItems.SO;
using Model.UpgradeShop;
using Presenter.SceneEntity;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Model.Infrastructure.Services.Factories
{
	public class UpgradeWindowFactory : IUpgradeWindowFactory
	{
		private GameObject _upgradeWindow;
		private readonly IAssetProvider _assetProvider;
		public UpgradeWindow UpgradeWindow { get; private set; }

		public UpgradeWindowFactory()
		{
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
			_upgradeWindow = await _assetProvider.Instantiate(ConstantNames.UIElementNames.UpgradeWindow);
			UpgradeWindow = _upgradeWindow.GetComponent<UpgradeWindow>();
			ShopItemFactory shopFactory = new ShopItemFactory();
			ShopItemList shopItemList = await shopFactory.GetItemList();

			List<UpgradeElement> buttons = InstantiateButtons(shopItemList);
			UpgradeWindow.Construct(buttons);
			return _upgradeWindow;
		}

		private List<UpgradeElement> InstantiateButtons(ShopItemList items)
		{
			List<UpgradeElement> buttons = new();
			
			foreach (var item in items.Items)
			{
				UpgradeElement element = Object.Instantiate(item.UpgradeElement,
					UpgradeWindow.UpgradeElementsContainer.transform);

				buttons.Add(element);
			}

			return buttons;
		}
	}
}