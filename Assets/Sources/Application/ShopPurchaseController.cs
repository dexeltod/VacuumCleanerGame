using System;
using System.Collections.Generic;
using System.Linq;
using Sources.DIService;
using Sources.InfrastructureInterfaces;
using Sources.PresetrationInterfaces;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.UI;
using Sources.View;

namespace Sources.Application
{
	public class ShopPurchaseController
	{
		private const int Point = 1;

		private readonly IReadOnlyList<IUpgradeItem> _items;
		private readonly List<IUpgradeElementView> _buttonElements;
		
		private readonly IUpgradeWindow _upgradeWindow;
		private readonly IResourcesProgressViewModel _resourcesProgress;
		private readonly IShopProgressViewModel _shopProgressViewModel;
		private readonly IPlayerProgressViewModel _playerProgress;

		public ShopPurchaseController(
			IReadOnlyList<IUpgradeItem> items,
			IUpgradeWindow upgradeWindow,
			List<IUpgradeElementView> upgradeElements
		)

		{
			_resourcesProgress = GameServices.Container.Get<IResourcesProgressViewModel>();
			_shopProgressViewModel = GameServices.Container.Get<IShopProgressViewModel>();
			_playerProgress = GameServices.Container.Get<IPlayerProgressViewModel>();

			_items = items;
			_upgradeWindow = upgradeWindow;

			Dictionary<string, int> upgrades = new();

			_buttonElements = upgradeElements;

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

		private void OnButtonPressed(IUpgradeItemView type)
		{
			if (_buttonElements.Contains((UpgradeElementView)type))
			{
				var a = _buttonElements.FirstOrDefault(x => x == type);
				
			}

			

			foreach (UpgradeElementView upgradeElement in _buttonElements)
			{
				if (upgradeElement.ItemData == type)
				{
					bool canUpgrade = CheckExceptions(upgradeElement);
					SetProgress(canUpgrade, upgradeElement);
					return;
				}
			}
		}

		private void SetProgress(bool canUpgrade, IUpgradeItem upgradeElement)
		{
			if (canUpgrade)
			{
				string progressName = upgradeElement.Name;

				_resourcesProgress.DecreaseMoney(upgradeElement.Price);
				SetUpgradeLevel(upgradeElement);
				AddProgressPoints(upgradeElement, progressName);

				_playerProgress.SetProgress(progressName);
			}
		}

		private void SetUpgradeLevel(IUpgradeItem upgradeElement)
		{
			int newLevel = upgradeElement.PointLevel + Point;
			upgradeElement.SetUpgradeLevel(newLevel);
		}

		private void AddProgressPoints(IUpgradeItem upgradeElement, string progressName)
		{
			_shopProgressViewModel.AddProgressPoint(progressName);
			upgradeElement.AddProgressPointColor(Point);
		}

		private bool CheckExceptions(IUpgradeItem upgradeElement)
		{
			if (_resourcesProgress.SoftCurrency.Count - upgradeElement.Price < 0)
				throw new InvalidOperationException("Not enough money");

			if (upgradeElement.PointLevel >= 6)
				throw new InvalidOperationException("Maximum upgrade level");

			return true;
		}
	}
}