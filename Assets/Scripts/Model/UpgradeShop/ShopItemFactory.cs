using Cysharp.Threading.Tasks;
using Model.DI;
using Model.Infrastructure.Services;
using UnityEngine;

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

		public async UniTask<ShopItemList> GetItemList()
		{
			GameObject gameObject = await _assetProvider.Instantiate(ShopItemList);
			ShopItemList list = gameObject.GetComponent<ShopItemList>();
			return list;
		}
	}
}