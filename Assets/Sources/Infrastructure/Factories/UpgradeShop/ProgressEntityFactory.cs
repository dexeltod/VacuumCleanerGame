using System;
using System.Collections.Generic;
using Sources.Infrastructure.Configs;
using Sources.Presentation.SceneEntity;
using Sources.ServicesInterfaces;
using Sources.Utils;
using VContainer;

namespace Sources.Infrastructure.Factories.UpgradeShop
{
	public class ProgressEntityFactory
	{
		private readonly LoadingCurtain _loadingCurtain;
		private readonly IAssetFactory _assetFactory;

		private IReadOnlyCollection<UpgradeEntityViewConfig> _items;
		private string UIResourcesShopItems => ResourcesAssetPath.Scene.UIResources.ShopItems;

		[Inject]
		public ProgressEntityFactory(IAssetFactory assetFactory) =>
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));

		public IReadOnlyCollection<UpgradeEntityViewConfig> Load()
		{
			if (_items != null)
				return _items;

			UpgradeEntityListConfig upgradeEntityListConfig
				= _assetFactory.LoadFromResources<UpgradeEntityListConfig>(UIResourcesShopItems);

			IReadOnlyCollection<UpgradeEntityViewConfig> upgradeItemData = upgradeEntityListConfig.ReadOnlyItems;

			_items = upgradeItemData ?? throw new NullReferenceException("ShopItems is null");

			return _items;
		}
	}
}