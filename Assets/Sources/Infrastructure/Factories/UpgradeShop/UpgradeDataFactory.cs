using System;

using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Upgrade;
using Sources.Presentation.SceneEntity;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs;
using Sources.Utils.Configs.Scripts;

namespace Sources.Infrastructure.Factories.UpgradeShop
{
	public class UpgradeDataFactory : IUpgradeDataFactory
	{
		private readonly LoadingCurtain _loadingCurtain;
		private readonly IAssetProvider _assetProvider;

		private IUpgradeItemData[] _items;

		public UpgradeDataFactory(IAssetProvider assetProvider) =>
			_assetProvider = assetProvider;

		public IUpgradeItemData[] LoadItems()
		{
			if (_items != null)
				return _items;

			UpgradeItemList upgradeItemList = _assetProvider
				.LoadFromResources<UpgradeItemList>(ResourcesAssetPath.GameObjects.ShopItems);

			IUpgradeItemData[] upgradeItemData = upgradeItemList.Items;

			_items = upgradeItemData ?? throw new NullReferenceException("ShopItems is null");

			return _items;
		}
	}
}