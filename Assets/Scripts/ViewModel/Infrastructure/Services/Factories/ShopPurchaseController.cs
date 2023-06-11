using System;
using System.Collections.Generic;
using Model.DI;
using Model.ScriptableObjects.UpgradeItems.SO;
using UnityEngine;
using View.SceneEntity;
using View.UI.Shop;

namespace ViewModel.Infrastructure.Services.Factories
{
	public class ShopPurchaseController
	{
		private const int PointCount = 1;

		private readonly UpgradeWindow _upgradeWindow;
		private readonly List<UpgradeElementView> _buttonElements;
		private readonly List<string> _buttonNames;
		private readonly IPlayerProgressViewModel _playerProgress;
		private readonly IShopProgressViewModel _shopProgress;

		public ShopPurchaseController(UpgradeWindow upgradeWindow, List<UpgradeElementView> upgradeElements,
			List<string> buttonNames)
		{
			_playerProgress = ServiceLocator.Container.GetSingle<IPlayerProgressViewModel>();
			_shopProgress = ServiceLocator.Container.GetSingle<IShopProgressViewModel>();

			_upgradeWindow = upgradeWindow;
			_buttonElements = upgradeElements;
			_buttonNames = buttonNames;

			_upgradeWindow.ActiveChanged += OnActiveChanged;
			_upgradeWindow.Destroyed += OnDestroyed;
		}

		private void OnActiveChanged(bool isActive)
		{
			if (isActive == true)
				SubscribeOnButtons(_buttonElements);
			else
				UnsubscribeButtons(_buttonElements);
		}

		private void OnDestroyed()
		{
			_upgradeWindow.Destroyed -= OnDestroyed;
			_upgradeWindow.ActiveChanged -= OnActiveChanged;
		}

		private void SubscribeOnButtons(List<UpgradeElementView> elements)
		{
			foreach (var element in elements)
				element.BuyButtonPressed += OnButtonPressed;
		}

		private void UnsubscribeButtons(List<UpgradeElementView> elements)
		{
			foreach (var element in elements)
				element.BuyButtonPressed -= OnButtonPressed;
		}

		private void OnButtonPressed(UpgradeItemScriptableObject.Upgrade type)
		{
			string buttonName = Enum.GetName(typeof(UpgradeItemScriptableObject.Upgrade), type);

			if (_buttonNames.Contains(buttonName))
				foreach (UpgradeElementView button in _buttonElements)
					if (button.ItemData.GetProgressName() == buttonName)
					{
						TryBuyUpgrade(button);
						return;
					}
		}

		private void TryBuyUpgrade(UpgradeElementView upgradeElement)
		{
			if (_playerProgress.Money - upgradeElement.ItemData.Price < 0)
				return;

			_playerProgress.DecreaseMoney(upgradeElement.ItemData.Price);
			_shopProgress.AddProgressPoint(upgradeElement.ItemData.GetProgressName());
			upgradeElement.AddProgressPointColor(PointCount);
		}
	}
}