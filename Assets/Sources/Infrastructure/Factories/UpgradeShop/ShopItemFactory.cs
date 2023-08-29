using System;
using Sources.Application.Utils.Configs;
using Sources.DIService;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Upgrade;
using Sources.ServicesInterfaces;

namespace Sources.Infrastructure.Factories.UpgradeShop
{
	public class ShopItemFactory : IShopItemFactory
	{
		private readonly IAssetProvider _assetProvider;

		private IUpgradeItemData[] _items;

		public ShopItemFactory()
		{
			_assetProvider = GameServices.Container.Get<IAssetProvider>();
		}

		public IUpgradeItemData[] LoadItems()
		{
			if (_items != null)
				return _items;

			UpgradeItemList upgradeItemList =
				_assetProvider.Load<UpgradeItemList>(ResourcesAssetPath.GameObjects.ShopItems);

			IUpgradeItemData[] upgradeItemData = upgradeItemList.Items;

			_items = upgradeItemData ?? throw new NullReferenceException("ShopItems is null");

			return _items;
		}
	}
}