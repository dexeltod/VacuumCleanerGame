using System;
using System.Collections.Generic;
using Sources.Core.DI;
using Sources.Infrastructure.InfrastructureInterfaces;
using Sources.Infrastructure.Services;
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
			_resourcesProgress = ServiceLocator.Container.Get<IResourcesProgressViewModel>();
			_shopProgressViewModel = ServiceLocator.Container.Get<IShopProgressViewModel>();
			_playerProgress = ServiceLocator.Container.Get<IPlayerProgressViewModel>();

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
				foreach (UpgradeElementView upgradeElement in _buttonElements)
					if (upgradeElement.ItemData.GetProgressName() == buttonName)
					{
						bool canUpgrade = CheckExceptions(upgradeElement);

						if (canUpgrade)
						{
							string progressName = upgradeElement.ItemData.GetProgressName();

							_resourcesProgress.DecreaseMoney(upgradeElement.ItemData.Price);
							int newLevel = upgradeElement.ItemData.UpgradeLevel + Point;
							upgradeElement.ItemData.SetUpgradeLevel(newLevel);

							_shopProgressViewModel.AddProgressPoint(progressName);
							upgradeElement.AddProgressPointColor(Point);
							
							_playerProgress.SetProgress(progressName);
						}

						return;
					}
		}

		private bool CheckExceptions(UpgradeElementView upgradeElement)
		{
			if (_resourcesProgress.SoftCurrency.Count - upgradeElement.ItemData.Price < 0)
				throw new InvalidOperationException("Not enough money");

			if (upgradeElement.ItemData.UpgradeLevel >= 6)
				throw new InvalidOperationException("Maximum upgrade level");

			return true;
		}
	}
}