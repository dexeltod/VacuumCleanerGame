using Sources.Application.Utils.Configs;
using Sources.DIService;
using Sources.Infrastructure.ScriptableObjects;
using Sources.InfrastructureInterfaces;
using Sources.ServicesInterfaces;

namespace Sources.Infrastructure.Factories.UpgradeShop
{
	public class ShopItemFactory : IShopItemFactory
	{
		private readonly IResourceProvider _assetProvider;

		private UpgradeItemList _items;

		public ShopItemFactory()
		{
			_assetProvider = GameServices.Container.Get<IResourceProvider>();
		}

		public IUpgradeItemList LoadItems()
		{
			if (_items != null)
				return _items;

			_items = _assetProvider.Load<UpgradeItemList>(ResourcesAssetPath.ShopConfig.ShopItems);
			return _items;
		}
	}
}