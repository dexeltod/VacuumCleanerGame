using System;
using System.Collections.Generic;
using System.Linq;
using Sources.DIService;
using Sources.InfrastructureInterfaces;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.UI;
using Sources.View;

namespace Sources.Application
{
	public class ShopPurchaseController
	{
		private const int Point = 1;

		private readonly List<UpgradeElementPrefab> _upgradeElements;

		private readonly IUpgradeWindow _upgradeWindow;
		private readonly IResourcesProgressViewModel _resourcesProgress;
		private readonly IShopProgressViewModel _shopProgressViewModel;
		private readonly IPlayerProgressViewModel _playerProgress;

		private readonly Dictionary<string, UpgradeElementPrefab> _prefabsByNames =
			new Dictionary<string, UpgradeElementPrefab>();

		public ShopPurchaseController
		(IUpgradeWindow upgradeWindow,
			List<UpgradeElementPrefab> upgradeElements
		)

		{
			_resourcesProgress = GameServices.Container.Get<IResourcesProgressViewModel>();
			_shopProgressViewModel = GameServices.Container.Get<IShopProgressViewModel>();
			_playerProgress = GameServices.Container.Get<IPlayerProgressViewModel>();

			_upgradeWindow = upgradeWindow;
			_upgradeElements = upgradeElements;

			foreach (UpgradeElementPrefab element in _upgradeElements)
				_prefabsByNames.Add(element.IdName, element);

			_upgradeWindow.ActiveChanged += OnActiveChanged;
			_upgradeWindow.Destroyed += OnDestroyed;
		}

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

		private void OnButtonPressed(IUpgradeItemData data)
		{
			if (IsHaveExceptions(data) == false)
				return;

			SetProgress(data);
			ChangeColor(data);
		}

		private void ChangeColor(IUpgradeItemData upgradeItemData)
		{
			IColorChangeable color = _prefabsByNames
				.FirstOrDefault(element => element.Key == upgradeItemData.IdName)
				.Value;
			
			color.AddProgressPointColor(upgradeItemData.PointLevel);
		}

		private void SetProgress(IUpgradeItemData upgradeElement)
		{
			_resourcesProgress.DecreaseMoney(upgradeElement.Price);
			SetUpgradeLevel(upgradeElement);
			_playerProgress.SetProgress(upgradeElement.IdName);
		}

		private void SetUpgradeLevel(IUpgradeItemData upgradeElement)
		{
			int newLevel = upgradeElement.PointLevel + Point;
			upgradeElement.SetUpgradeLevel(newLevel);
		}

		private bool IsHaveExceptions(IUpgradeItemData upgradeElement)
		{
			if (_resourcesProgress.SoftCurrency.Count - upgradeElement.Price < 0)
				throw new InvalidOperationException("Not enough money");

			if (upgradeElement.PointLevel >= 6)
				throw new InvalidOperationException("Maximum upgrade level");

			return true;
		}
	}
}