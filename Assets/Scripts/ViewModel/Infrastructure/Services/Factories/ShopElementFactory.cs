using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Model.Data;
using Model.DI;
using Model.UpgradeShop;
using UnityEngine;
using View.UI.Shop;
using Object = UnityEngine.Object;

namespace ViewModel.Infrastructure.Services.Factories
{
	public class ShopElementFactory
	{
		private const string ShopItemList = "ShopItemList";
		
		private readonly ShopProgress _shopProgress;
		private readonly IAssetProvider _assetProvider;

		public ShopElementFactory(ShopProgress shopProgress)
		{
			_shopProgress = shopProgress;
			_assetProvider = ServiceLocator.Container.GetSingle<IAssetProvider>();
		}

		public async UniTask<List<UpgradeElementView>> InstantiateElements(Transform transform)
		{
			List<UpgradeElementView> buttons = new();

			List<Tuple<string, int>> progress = _shopProgress.GetAllProgress();

			var items = await _assetProvider.LoadAsync<ShopItemList>(ShopItemList);
			
			Instantiate(transform,  items, buttons, progress);
			return buttons;
		}

		private void Instantiate(Transform transform, ShopItemList items, List<UpgradeElementView> buttons,
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