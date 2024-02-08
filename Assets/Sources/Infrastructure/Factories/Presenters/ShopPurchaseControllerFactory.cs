using System;
using System.Collections.Generic;
using Sources.Controllers;
using Sources.Infrastructure.Common.Factory.Decorators;
using Sources.InfrastructureInterfaces.Presenters;
using Sources.Presentation.UI.Shop;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.DTO;
using Sources.Utils.Configs.Scripts;

namespace Sources.Infrastructure.Factories.Presenters
{
	public class ShopPurchaseControllerFactory : PresenterFactory<ShopPurchasePresenter>
	{
		private readonly IAssetFactory _assetFactory;
		private readonly List<IUpgradeElementPrefabView> _upgradeElementsPrefabs;
		private readonly IResourcesProgressPresenter _resourceProgressPresenter;
		private readonly IShopProgressProvider _shopProgressProvider;
		private readonly IPlayerProgressProvider _playerProgressProvider;
		private readonly IGameplayInterfaceView _gameplayInterfaceView;

		private string UpgradeWindowPath => ResourcesAssetPath.Scene.UIResources.UpgradeWindow;

		public ShopPurchaseControllerFactory(
			IAssetFactory assetFactory,
			List<IUpgradeElementPrefabView> upgradeElementsPrefabs,
			IResourcesProgressPresenter resourceProgressPresenter,
			IShopProgressProvider shopProgressProvider,
			IPlayerProgressProvider playerProgressProvider,
			IGameplayInterfaceView gameplayInterfaceView
		)
		{
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_upgradeElementsPrefabs = upgradeElementsPrefabs ??
				throw new ArgumentNullException(nameof(upgradeElementsPrefabs));
			_resourceProgressPresenter = resourceProgressPresenter ??
				throw new ArgumentNullException(nameof(resourceProgressPresenter));
			_shopProgressProvider
				= shopProgressProvider ?? throw new ArgumentNullException(nameof(shopProgressProvider));
			_playerProgressProvider = playerProgressProvider ??
				throw new ArgumentNullException(nameof(playerProgressProvider));
			_gameplayInterfaceView
				= gameplayInterfaceView ?? throw new ArgumentNullException(nameof(gameplayInterfaceView));
		}

		public override ShopPurchasePresenter Create() =>
			new ShopPurchasePresenter(
				_upgradeElementsPrefabs,
				_resourceProgressPresenter,
				_shopProgressProvider,
				_playerProgressProvider,
				_gameplayInterfaceView
			);
	}
}