using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sources.Controllers.Common;
using Sources.Presentation.UI.Shop;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.DTO;
using Sources.ServicesInterfaces.Upgrade;

namespace Sources.Controllers
{
	public class ShopPurchasePresenter : Presenter
	{
		private const int Point = 1;

		private readonly List<IUpgradeElementPrefabView> _upgradeElements;

		private readonly IResourcesProgressPresenter _resourcesProgress;
		private readonly IPlayerProgressProvider _playerProgress;
		private readonly IGameplayInterfaceView _gameplayInterfaceView;
		private readonly IShopProgressProvider _shopProgressProvider;
		private readonly IUpgradeWindow _upgradeWindow;

		private readonly Dictionary<string, IUpgradeElementPrefabView> _prefabsByNames =
			new Dictionary<string, IUpgradeElementPrefabView>();

		private bool _isCanAddProgress = true;

		public ShopPurchasePresenter(
			IUpgradeWindow upgradeWindow,
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

			_upgradeWindow = upgradeWindow ?? throw new ArgumentNullException(nameof(upgradeWindow));
			_upgradeElements = upgradeElements ?? throw new ArgumentNullException(nameof(upgradeElements));

			foreach (IUpgradeElementPrefabView element in _upgradeElements)
				_prefabsByNames.Add(element.IdName, element);
		}

		public override void Enable()
		{
			_upgradeElements.SubscribeOnButtons(_upgradeElements);
			ChangeActiveGameplayInterface(false);
		}

		public override void Disable()
		{
			UnsubscribeFromButtons(_upgradeElements);
			ChangeActiveGameplayInterface(true);
		}

	

		private void ChangeActiveGameplayInterface(bool isEnabled)
		{
			if (_gameplayInterfaceView != null)
				_gameplayInterfaceView.Canvas.enabled = isEnabled;
		}

		private async void OnButtonPressed(IUpgradeItemData itemsData)
		{
			CheckExceptions(itemsData);

			await SetProgress(itemsData);

			ChangeColor(itemsData);
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

		private void ChangeColor(string idName)
		{
			IColorChangeable color = _prefabsByNames
				.FirstOrDefault(element => element.Key == _upgradeElements.IdName == idName)
				.Value;

			color.AddProgressPointColor(Point);
		}

		
	}
}