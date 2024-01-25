using System;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Upgrade;
using Sources.Presentation.SceneEntity;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs.Scripts;
using VContainer;

namespace Sources.Infrastructure.Factories.UpgradeShop
{
	public class ProgressUpgradeFactory : IProgressUpgradeFactory
	{
		private readonly LoadingCurtain _loadingCurtain;
		private readonly IAssetProvider _assetProvider;

		private IUpgradeItemData[] _items;

		[Inject]
		public ProgressUpgradeFactory(IAssetProvider assetProvider) =>
			_assetProvider = assetProvider;

		public IUpgradeItemData[] LoadItems()
		{
			if (_items != null)
				return _items;

			UpgradeItemList upgradeItemList = _assetProvider
				.LoadFromResources<UpgradeItemList>(ResourcesAssetPath.Scene.UIResources.ShopItems);

			IUpgradeItemData[] upgradeItemData = upgradeItemList.Items;

			_items = upgradeItemData ?? throw new NullReferenceException("ShopItems is null");

			return _items;
		}
	}
}