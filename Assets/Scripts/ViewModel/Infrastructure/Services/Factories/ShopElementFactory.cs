using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Model.Infrastructure.Data;
using Model.UpgradeShop;
using UnityEngine;
using View.UI.Shop;
using Object = UnityEngine.Object;

namespace ViewModel.Infrastructure.Services.Factories
{
	public class ShopElementFactory
	{
		private readonly ShopProgress _shopProgress;
		private readonly ShopItemFactory _shopFactory;
		
		public ShopElementFactory(ShopProgress shopProgress)
		{
			_shopProgress = shopProgress;
			_shopFactory = new ShopItemFactory();
		}

		public async UniTask<List<UpgradeElementView>> InstantiateElements(Transform transform)
		{
			List<UpgradeElementView> buttons = new();
			ShopItemList items = await _shopFactory.InitializeItemsList(_shopProgress);

			List<Tuple<string, int>> progress = _shopProgress.LoadProgress();

			Instantiate(transform, items, buttons, progress);
			return buttons;
		}

		private void Instantiate(Transform transform, ShopItemList items, List<UpgradeElementView> buttons,
			List<Tuple<string, int>> progress)
		{
			buttons.AddRange(items.Items.Select((item, i) =>
				Object.Instantiate(item.UpgradeElementView, transform.transform)
				.Construct(item, progress[i].Item2)));
		}
	}
}