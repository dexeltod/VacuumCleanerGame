using System;
using Sources.Infrastructure.Configs.Scripts;
using Sources.InfrastructureInterfaces.Factory;
using Sources.Presentation.SceneEntity;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.Upgrade;
using VContainer;

namespace Sources.Infrastructure.Factories.UpgradeShop
{
	public class ProgressUpgradeFactory : IProgressUpgradeFactory
	{
		private readonly LoadingCurtain _loadingCurtain;
		private readonly IAssetFactory _assetFactory;

		private IUpgradeItemData[] _items;
		private string UIResourcesShopItems => ResourcesAssetPath.Scene.UIResources.ShopItems;

		[Inject]
		public ProgressUpgradeFactory(IAssetFactory assetFactory) =>
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));

		public IUpgradeItemData[] LoadItems()
		{
			if (_items != null)
				return _items;

			UpgradeItemList upgradeItemList = _assetFactory.LoadFromResources<UpgradeItemList>(UIResourcesShopItems);

			IUpgradeItemData[] upgradeItemData = upgradeItemList.Items;

			_items = upgradeItemData ?? throw new NullReferenceException("ShopItems is null");

			return _items;
		}
	}
}