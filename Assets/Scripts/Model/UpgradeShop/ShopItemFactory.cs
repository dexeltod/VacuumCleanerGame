using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Model.DI;
using Model.Infrastructure.Data;
using UnityEngine;
using ViewModel.Infrastructure.Services;

namespace Model.UpgradeShop
{
	public class ShopItemFactory
	{
		private const string ShopItemList = "ShopItemList";
		private readonly IAssetProvider _assetProvider;

		private ShopItemList _items;

		public ShopItemFactory()
		{
			_assetProvider = ServiceLocator.Container.GetSingle<IAssetProvider>();
		}

		public async UniTask<ShopItemList> InitializeItemsList(ShopProgress shopProgress)
		{
			ShopItemList list = await GetItems();
			InitializeProgress(list, shopProgress);
			return list;
		}

		private void InitializeProgress(ShopItemList list, ShopProgress shopProgress)
		{
			List<string> shopItemNames = new List<string>();

			foreach (var item in list.Items)
				shopItemNames.Add(item.GetProgressName());

			shopProgress.InitializeProgress(shopItemNames);
		}

		private async Task<ShopItemList> GetItems()
		{
			GameObject gameObject = await _assetProvider.Instantiate(ShopItemList);
			ShopItemList list = gameObject.GetComponent<ShopItemList>();
			return list;
		}
	}
}