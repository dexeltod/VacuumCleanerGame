using Cysharp.Threading.Tasks;
using Sources.Core.DI;
using Sources.Infrastructure.Services.Interfaces;

namespace Sources.Core.Application.UpgradeShop
{
	public class ShopItemFactory
	{
		private const string ShopItemList = "ShopItemList";
		private readonly IAssetProvider _assetProvider;

		private UpgradeItemList _items;

		public ShopItemFactory()
		{
			_assetProvider = ServiceLocator.Container.Get<IAssetProvider>();
		}

		public async UniTask<UpgradeItemList> LoadItems() =>
			await _assetProvider.LoadAsync<UpgradeItemList>(ShopItemList);
	}
}