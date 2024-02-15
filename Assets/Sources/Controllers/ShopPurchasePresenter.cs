using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces.DTO;
using Sources.ServicesInterfaces.Upgrade;

namespace Sources.Controllers
{
	// public sealed class ShopPurchasePresenter : Presenter, IShopPurchasePresenter
	// {
	// 	private const int Point = 1;
	//
	// 	private readonly List<IUpgradeElementPrefabView> _upgradeElements;
	//
	// 	private readonly IResourcesProgressPresenter _resourcesProgress;
	// 	private readonly IPlayerProgressSetterFacade _playerProgress;
	// 	private readonly IGameplayInterfaceView _gameplayInterfaceView;
	// 	private readonly IShopProgressFacade _shopProgressFacade;
	//
	// 	private bool _isCanAddProgress = true;
	//
	// 	public ShopPurchasePresenter(
	// 		List<IUpgradeElementPrefabView> upgradeElements,
	// 		IResourcesProgressPresenter resourcesProgress,
	// 		IShopProgressFacade shopProgressFacade,
	// 		IPlayerProgressSetterFacade playerProgressSetterFacade,
	// 		IGameplayInterfaceView gameplayInterfaceView
	// 	)
	// 	{
	// 		_resourcesProgress = resourcesProgress ?? throw new ArgumentNullException(nameof(resourcesProgress));
	// 		_shopProgressFacade
	// 			= shopProgressFacade ?? throw new ArgumentNullException(nameof(shopProgressFacade));
	// 		_playerProgress = playerProgressSetterFacade ?? throw new ArgumentNullException(nameof(playerProgressSetterFacade));
	// 		_gameplayInterfaceView
	// 			= gameplayInterfaceView ?? throw new ArgumentNullException(nameof(gameplayInterfaceView));
	//
	// 		_upgradeElements = upgradeElements ?? throw new ArgumentNullException(nameof(upgradeElements));
	// 	}
	//
	// 	public override void Enable() =>
	// 		ChangeActiveGameplayInterface(false);
	//
	// 	public override void Disable() =>
	// 		ChangeActiveGameplayInterface(true);
	//
	// 	private void ChangeActiveGameplayInterface(bool isEnabled)
	// 	{
	// 		if (_gameplayInterfaceView != null)
	// 			_gameplayInterfaceView.Canvas.enabled = isEnabled;
	// 	}
	// 	public void ChangeColor(string idName)
	// 	{
	// 		IColorChangeable color = _upgradeElements
	// 			.FirstOrDefault(element => element.IdName == idName);
	//
	// 		color?.AddProgressPointColor(Point);
	// 	}
	// }
}