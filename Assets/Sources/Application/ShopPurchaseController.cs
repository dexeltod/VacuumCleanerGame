using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sources.DIService;
using Sources.InfrastructureInterfaces.DTO;
using Sources.InfrastructureInterfaces.Upgrade;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.UI;
using Sources.View.UI.Shop;

namespace Sources.Application
{
	public class ShopPurchaseController : IDisposable
	{
		private const int Point = 1;

		private readonly List<UpgradeElementPrefab> _upgradeElements;

		private readonly IUpgradeWindow _upgradeWindow;
		private readonly IResourcesProgressPresenter _resourcesProgress;
		private readonly IShopProgressProvider _shopProgressProvider;
		private readonly IPlayerProgressProvider _playerProgress;

		private readonly Dictionary<string, UpgradeElementPrefab> _prefabsByNames =
			new Dictionary<string, UpgradeElementPrefab>();

		private bool _isCanAddProgress;

		public ShopPurchaseController
		(
			IUpgradeWindow upgradeWindow,
			List<UpgradeElementPrefab> upgradeElements
		)
		{
			_resourcesProgress = GameServices.Container.Get<IResourcesProgressPresenter>();
			_shopProgressProvider = GameServices.Container.Get<IShopProgressProvider>();
			_playerProgress = GameServices.Container.Get<IPlayerProgressProvider>();

			_upgradeWindow = upgradeWindow;
			_upgradeElements = upgradeElements;

			foreach (UpgradeElementPrefab element in _upgradeElements)
				_prefabsByNames.Add(element.IdName, element);

			_upgradeWindow.ActiveChanged += OnActiveChanged;
			_upgradeWindow.Destroyed += OnDestroyed;
		}

		public void Dispose() =>
			UnsubscribeFromButtons(_upgradeElements);

		private void OnActiveChanged(bool isActive)
		{
			if (isActive == true)
				SubscribeOnButtons(_upgradeElements);
			else
				UnsubscribeFromButtons(_upgradeElements);
		}

		private void OnDestroyed()
		{
			_upgradeWindow.Destroyed -= OnDestroyed;
			_upgradeWindow.ActiveChanged -= OnActiveChanged;
		}

		private void SubscribeOnButtons(List<UpgradeElementPrefab> elements)
		{
			foreach (var element in elements)
				element.BuyButtonPressed += OnButtonPressed;
		}

		private void UnsubscribeFromButtons(List<UpgradeElementPrefab> elements)
		{
			foreach (var element in elements)
				element.BuyButtonPressed -= OnButtonPressed;
		}

		private async void OnButtonPressed(IUpgradeItemData data)
		{
			if (_isCanAddProgress == false)
				throw new InvalidOperationException("Callback still not received");

			if (data.PointLevel >= data.MaxPointLevel)
				throw new InvalidOperationException("Maximum upgrade level");

			await SetProgress(data);

			ChangeColor(data);
		}

		private void ChangeColor(IUpgradeItemData upgradeItemData)
		{
			IColorChangeable color = _prefabsByNames
				.FirstOrDefault(element => element.Key == upgradeItemData.IdName)
				.Value;

			color.AddProgressPointColor(Point);
		}

		private async UniTask SetProgress(IUpgradeItemData upgradeElement)
		{
			_isCanAddProgress = false;

			int countedMoney = _resourcesProgress.GetDecreasedMoney(upgradeElement.Price);

			if (countedMoney < 0)
				throw new InvalidOperationException("Money less than zero");

			await _shopProgressProvider.AddProgressPoint
			(
				upgradeElement.IdName,
				succededCallback: () =>
				{
					_playerProgress.SetProgress(upgradeElement.IdName);
					_isCanAddProgress = true;
				}
			);

			SetUpgradeLevelVisual(upgradeElement);
		}

		private void SetUpgradeLevelVisual(IUpgradeItemData upgradeElement)
		{
			int newLevel = upgradeElement.PointLevel + Point;
			upgradeElement.SetUpgradeLevel(newLevel);
		}
	}
}