using Application.DI;
using Cysharp.Threading.Tasks;
using InfrastructureInterfaces;

namespace Application.UpgradeShop
{
	public class ShopItemFactory
	{
		private const string ShopItemList = "ShopItemList";
		private readonly IAssetProvider _assetProvider;

		private UpgradeItemList _items;

		public ShopItemFactory()
		{
			_assetProvider = ServiceLocator.Container.GetSingle<IAssetProvider>();
		}

		public async UniTask<UpgradeItemList> LoadItems() =>
			await _assetProvider.LoadAsync<UpgradeItemList>(ShopItemList);
	}
}