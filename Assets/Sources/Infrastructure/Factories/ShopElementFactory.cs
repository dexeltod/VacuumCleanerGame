using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sources.Core.Application.UpgradeShop;
using Sources.Core.DI;
using Sources.Core.Domain.Progress;
using Sources.Infrastructure.Services.Interfaces;
using Sources.View.ScriptableObjects.UpgradeItems.SO;
using Sources.View.UI.Shop;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sources.Infrastructure.Factories
{
	public class ShopElementFactory
	{
		private const string ShopItemList = "ShopItemList";
		
		private readonly ShopProgress _shopProgress;
		private readonly IAssetProvider _assetProvider;

		public ShopElementFactory(ShopProgress shopProgress)
		{
			_shopProgress = shopProgress;
			_assetProvider = ServiceLocator.Container.Get<IAssetProvider>();
		}

		public async UniTask<List<UpgradeElementView>> InstantiateElements(Transform transform)
		{
			List<UpgradeElementView> buttons = new();

			List<Tuple<string, int>> progress = _shopProgress.GetAll();

			UpgradeItemList items = await _assetProvider.LoadAsync<UpgradeItemList>(ShopItemList);

			InitItems(progress, items);
			
			Instantiate(transform,  items, buttons, progress);
			return buttons;
		}

		private void InitItems(List<Tuple<string, int>> progress, UpgradeItemList upgradeItems)
		{
			for (var i = 0; i < upgradeItems.Items.Count; i++)
			{
				UpgradeItemScriptableObject item = upgradeItems.Items[i];
				item.SetUpgradeLevel(progress[i].Item2);
			}
		}

		private void Instantiate(Transform transform, UpgradeItemList items, List<UpgradeElementView> buttons,
			List<Tuple<string, int>> progress)
		{
			for (int i = 0; i < progress.Count; i++)
			{
				var button = Object.Instantiate(items.Items[i].UpgradeElementView, transform.transform)
					.Construct(items.Items[i], progress[i].Item2);
				
				buttons.Add(button);
			}
		}
	}
}