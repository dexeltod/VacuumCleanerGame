using System;
using Sources.InfrastructureInterfaces.Factory;
using Sources.Presentation.SceneEntity;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.Upgrade;
using Sources.Utils.Configs.Scripts;
using VContainer;

namespace Sources.Infrastructure.Factories.UpgradeShop
{
	public class ProgressUpgradeFactory : IProgressUpgradeFactory
	{
		private readonly LoadingCurtain _loadingCurtain;
		private readonly IAssetResolver _assetResolver;

		private IUpgradeItemData[] _items;
		private string UIResourcesShopItems => ResourcesAssetPath.Scene.UIResources.ShopItems;

		[Inject]
		public ProgressUpgradeFactory(IAssetResolver assetResolver) =>
			_assetResolver = assetResolver ?? throw new ArgumentNullException(nameof(assetResolver));

		public IUpgradeItemData[] LoadItems()
		{
			if (_items != null)
				return _items;

			UpgradeItemList upgradeItemList = _assetResolver.LoadFromResources<UpgradeItemList>(UIResourcesShopItems);

			IUpgradeItemData[] upgradeItemData = upgradeItemList.Items;

			_items = upgradeItemData ?? throw new NullReferenceException("ShopItems is null");

			return _items;
		}
	}
}