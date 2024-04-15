using System;
using System.Collections.Generic;
using Sources.Infrastructure.Configs;
using Sources.Infrastructure.Configs.Scripts;
using Sources.Infrastructure.ScriptableObjects;
using Sources.InfrastructureInterfaces.Factory;
using Sources.Presentation.SceneEntity;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.Upgrade;
using Sources.Utils;
using VContainer;

namespace Sources.Infrastructure.Factories.UpgradeShop
{
	public class ProgressUpgradeFactory
	{
		private readonly LoadingCurtain _loadingCurtain;
		private readonly IAssetFactory _assetFactory;

		private IReadOnlyCollection<UpgradeItemViewConfig> _items;
		private string UIResourcesShopItems => ResourcesAssetPath.Scene.UIResources.ShopItems;

		[Inject]
		public ProgressUpgradeFactory(IAssetFactory assetFactory) =>
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));

		public IReadOnlyCollection<UpgradeItemViewConfig> LoadItems()
		{
			if (_items != null)
				return _items;

			UpgradeItemListConfig upgradeItemListConfig
				= _assetFactory.LoadFromResources<UpgradeItemListConfig>(UIResourcesShopItems);

			IReadOnlyCollection<UpgradeItemViewConfig> upgradeItemData = upgradeItemListConfig.ReadOnlyItems;

			_items = upgradeItemData ?? throw new NullReferenceException("ShopItems is null");

			return _items;
		}
	}
}