using System;
using Sources.DIService;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Upgrade;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs;
using Sources.View.SceneEntity;

namespace Sources.Infrastructure.Factories.UpgradeShop
{
	public class ShopItemFactory : IShopItemFactory
	{
		private readonly LoadingCurtain _loadingCurtain;
		private readonly IAssetProvider _assetProvider;

		private IUpgradeItemData[] _items;

		public ShopItemFactory(LoadingCurtain loadingCurtain)
		{
			_loadingCurtain = loadingCurtain;
			_assetProvider = GameServices.Container.Get<IAssetProvider>();
		}

		public IUpgradeItemData[] LoadItems()
		{
			if (_items != null)
				return _items;

			_loadingCurtain.SetText("Loading upgrade item list from assets");
			UpgradeItemList upgradeItemList = _assetProvider.Load<UpgradeItemList>(ResourcesAssetPath.GameObjects.ShopItems);

			_loadingCurtain.SetText("Items loaded");
			IUpgradeItemData[] upgradeItemData = upgradeItemList.Items;

			_items = upgradeItemData ?? throw new NullReferenceException("ShopItems is null");

			return _items;
		}
	}
}