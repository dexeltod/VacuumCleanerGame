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
	public sealed class ShopPurchasePresenter : Presenter, IShopPurchasePresenter
	{
		private const int Point = 1;

		private readonly List<IUpgradeElementPrefabView> _upgradeElements;

		private readonly IResourcesProgressPresenter _resourcesProgress;
		private readonly IPlayerProgressProvider _playerProgress;
		private readonly IGameplayInterfaceView _gameplayInterfaceView;
		private readonly IShopProgressProvider _shopProgressProvider;

		private bool _isCanAddProgress = true;

		public ShopPurchasePresenter(
			List<IUpgradeElementPrefabView> upgradeElements,
			IResourcesProgressPresenter resourcesProgress,
			IShopProgressProvider shopProgressProvider,
			IPlayerProgressProvider playerProgressProvider,
			IGameplayInterfaceView gameplayInterfaceView
		)
		{
			_resourcesProgress = resourcesProgress ?? throw new ArgumentNullException(nameof(resourcesProgress));
			_shopProgressProvider
				= shopProgressProvider ?? throw new ArgumentNullException(nameof(shopProgressProvider));
			_playerProgress = playerProgressProvider ?? throw new ArgumentNullException(nameof(playerProgressProvider));
			_gameplayInterfaceView
				= gameplayInterfaceView ?? throw new ArgumentNullException(nameof(gameplayInterfaceView));

			_upgradeElements = upgradeElements ?? throw new ArgumentNullException(nameof(upgradeElements));
		}

		public override void Enable() =>
			ChangeActiveGameplayInterface(false);

		public override void Disable() =>
			ChangeActiveGameplayInterface(true);

		private void ChangeActiveGameplayInterface(bool isEnabled)
		{
			if (_gameplayInterfaceView != null)
				_gameplayInterfaceView.Canvas.enabled = isEnabled;
		}

		private async void OnButtonPressed(IUpgradeItemData itemsData)
		{
			CheckExceptions(itemsData);

			await SetProgress(itemsData);

			ChangeColor(itemsData.IdName);
		}

		private async UniTask SetProgress(IUpgradeItemData upgradeElement)
		{
			_isCanAddProgress = false;

			await _shopProgressProvider.AddProgressPoint(
				upgradeElement.IdName,
				succeededCallback: () =>
				{
					_playerProgress.SetProgress(upgradeElement.IdName);
					_resourcesProgress.DecreaseMoney(upgradeElement.Price);
					_isCanAddProgress = true;
				}
			);

			upgradeElement.SetUpgradeLevel(upgradeElement.PointLevel + Point);
		}

		public void ChangeColor(string idName)
		{
			IColorChangeable color = _upgradeElements
				.FirstOrDefault(element => element.IdName == idName);

			color?.AddProgressPointColor(Point);
		}

		private void CheckExceptions(IUpgradeItemData item)
		{
			if (_isCanAddProgress == false)
				throw new InvalidOperationException("Callback still not received");

			if (item.PointLevel >= item.MaxPointLevel)
				throw new InvalidOperationException("Maximum upgrade level");

			int countedMoney = _resourcesProgress.GetDecreasedMoney(item.Price);

			if (countedMoney < 0)
				throw new InvalidOperationException("Not enough money");
		}
	}
}