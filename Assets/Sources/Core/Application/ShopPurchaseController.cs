using System;
using System.Collections.Generic;
using Sources.Core.DI;
using Sources.Infrastructure.InfrastructureInterfaces;
using Sources.View.Interfaces;
using Sources.View.SceneEntity;
using Sources.View.ScriptableObjects.UpgradeItems.SO;
using Sources.View.UI.Shop;

namespace Sources.Infrastructure.Factories
{
	public class ShopPurchaseController
	{
		private const int Point = 1;

		private readonly IUpgradeWindow _upgradeWindow;
		private readonly List<UpgradeElementView> _buttonElements;
		private readonly List<string> _buttonNames;
		private readonly IResourcesProgressViewModel _resourcesProgress;
		private readonly IShopProgressViewModel _shopProgressViewModel;
		private readonly IPlayerProgressViewModel _playerProgress;

		public ShopPurchaseController(IUpgradeWindow upgradeWindow, List<UpgradeElementView> upgradeElements,
			List<string> buttonNames)
		{
			_resourcesProgress = ServiceLocator.Container.GetSingle<IResourcesProgressViewModel>();
			_shopProgressViewModel = ServiceLocator.Container.GetSingle<IShopProgressViewModel>();
			_playerProgress = ServiceLocator.Container.GetSingle<IPlayerProgressViewModel>();

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
				UnsubscribeFromButtons(_buttonElements);
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

		private void UnsubscribeFromButtons(List<UpgradeElementView> elements)
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
			TryShowExceptions(upgradeElement);

			string progressName = upgradeElement.ItemData.GetProgressName();
			
			int newLevel = upgradeElement.ItemData.UpgradeLevel + Point;
			upgradeElement.ItemData.SetUpgradeLevel(newLevel);
			
			_resourcesProgress.DecreaseMoney(upgradeElement.ItemData.Price);
			_playerProgress.SetProgress(progressName);
			_shopProgressViewModel.AddProgressPoint(progressName);

			upgradeElement.AddProgressPointColor(Point);
		}

		private void TryShowExceptions(UpgradeElementView upgradeElement)
		{
			if (_resourcesProgress.SoftCurrency.Count - upgradeElement.ItemData.Price < 0)
				throw new InvalidOperationException("Not enough money");

			if (upgradeElement.ItemData.UpgradeLevel >= 6)
				throw new InvalidOperationException("Maximum upgrade level");
		}
	}
}