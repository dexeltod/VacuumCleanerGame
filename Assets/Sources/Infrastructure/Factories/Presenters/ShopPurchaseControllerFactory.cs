namespace Sources.Infrastructure.Factories.Presenters
{
	// public class ShopPurchaseControllerFactory : PresenterFactory<ShopPurchasePresenter>
	// {
	// 	private readonly IAssetFactory _assetFactory;
	// 	private readonly List<IUpgradeElementPrefabView> _upgradeElementsPrefabs;
	// 	private readonly IResourcesProgressPresenter _resourceProgressPresenter;
	// 	private readonly IShopProgressFacade _shopProgressFacade;
	// 	private readonly IPlayerProgressSetterFacadeProvider _playerProgressSetterFacade;
	// 	private readonly IGameplayInterfaceView _gameplayInterfaceView;
	//
	// 	private string UpgradeWindowPath => ResourcesAssetPath.Scene.UIResources.UpgradeWindow;
	//
	// 	public ShopPurchaseControllerFactory(
	// 		IAssetFactory assetFactory,
	// 		List<IUpgradeElementPrefabView> upgradeElementsPrefabs,
	// 		IResourcesProgressPresenter resourceProgressPresenter,
	// 		IShopProgressFacade shopProgressFacade,
	// 		IPlayerProgressSetterFacadeProvider playerProgressSetterFacade,
	// 		IGameplayInterfaceView gameplayInterfaceView
	// 	)
	// 	{
	// 		_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
	// 		_upgradeElementsPrefabs = upgradeElementsPrefabs ??
	// 			throw new ArgumentNullException(nameof(upgradeElementsPrefabs));
	// 		_resourceProgressPresenter = resourceProgressPresenter ??
	// 			throw new ArgumentNullException(nameof(resourceProgressPresenter));
	// 		_shopProgressFacade
	// 			= shopProgressFacade ?? throw new ArgumentNullException(nameof(shopProgressFacade));
	// 		_playerProgressSetterFacade = playerProgressSetterFacade ??
	// 			throw new ArgumentNullException(nameof(playerProgressSetterFacade));
	// 		_gameplayInterfaceView
	// 			= gameplayInterfaceView ?? throw new ArgumentNullException(nameof(gameplayInterfaceView));
	// 	}
	//
	// 	public override ShopPurchasePresenter Create() =>
	// 		new ShopPurchasePresenter(
	// 			_upgradeElementsPrefabs,
	// 			_resourceProgressPresenter,
	// 			_shopProgressFacade,
	// 			_playerProgressSetterFacade.Implementation,
	// 			_gameplayInterfaceView
	// 		);
	// }
}