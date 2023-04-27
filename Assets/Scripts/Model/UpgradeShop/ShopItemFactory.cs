using Cysharp.Threading.Tasks;
using Model.DI;
using Model.Infrastructure.Services;

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

		private async UniTask<ShopItemList> GetItemContainer() => 
			await _assetProvider.LoadAsyncByGUID<ShopItemList>(ShopItemList);
	}
}