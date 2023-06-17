using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Model.Data;
using Model.DI;
using Model.ScriptableObjects.UpgradeItems.SO;
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

		public async UniTask<ShopItemList> LoadItems()
		{
			ShopItemList list = await GetShopItemList();
			return list;
		}

		private async Task<ShopItemList> GetShopItemList()
		{
			var @object = await _assetProvider.LoadAsync<ShopItemList>(ShopItemList);
			ShopItemList list = @object;
			return list;
		}
	}
}