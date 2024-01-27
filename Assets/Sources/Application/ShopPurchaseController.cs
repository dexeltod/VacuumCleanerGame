using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Sources.InfrastructureInterfaces.DTO;
using Sources.InfrastructureInterfaces.Upgrade;
using Sources.Presentation.UI.Shop;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.UI;

namespace Sources.Application
{
	public class ShopPurchaseController : IDisposable
	{
		private const int Point = 1;

		private readonly List<UpgradeElementPrefabView> _upgradeElements;

		private readonly IResourcesProgressPresenter _resourcesProgress;
		private readonly IPlayerProgressProvider _playerProgress;
		private readonly IShopProgressProvider _shopProgressProvider;
		private readonly IUpgradeWindow _upgradeWindow;

		private readonly Dictionary<string, UpgradeElementPrefabView> _prefabsByNames =
			new Dictionary<string, UpgradeElementPrefabView>();

		private bool _isCanAddProgress = true;

		public ShopPurchaseController(
			IUpgradeWindow upgradeWindow,
			List<UpgradeElementPrefabView> upgradeElements,
			IResourcesProgressPresenter resourcesProgress,
			IShopProgressProvider shopProgressProvider,
			IPlayerProgressProvider playerProgressProvider
		)
		{
			_resourcesProgress = resourcesProgress ?? throw new ArgumentNullException(nameof(resourcesProgress));
			_shopProgressProvider = shopProgressProvider ?? throw new ArgumentNullException(nameof(shopProgressProvider));
			_playerProgress = playerProgressProvider ?? throw new ArgumentNullException(nameof(playerProgressProvider));

			_upgradeWindow = upgradeWindow ?? throw new ArgumentNullException(nameof(upgradeWindow));
			_upgradeElements = upgradeElements ?? throw new ArgumentNullException(nameof(upgradeElements));

			foreach (UpgradeElementPrefabView element in _upgradeElements)
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

		private void SubscribeOnButtons([NotNull] List<UpgradeElementPrefabView> elements)
		{
			if (elements == null) throw new ArgumentNullException(nameof(elements));

			foreach (UpgradeElementPrefabView element in elements)
				element.BuyButtonPressed += OnButtonPressed;
		}

		private void UnsubscribeFromButtons([NotNull] List<UpgradeElementPrefabView> elements)
		{
			if (elements == null) throw new ArgumentNullException(nameof(elements));

			foreach (UpgradeElementPrefabView element in elements)
				element.BuyButtonPressed -= OnButtonPressed;
		}

		private async void OnButtonPressed(IUpgradeItemData data)
		{
			CheckExceptions(data);

			await SetProgress(data);

			ChangeColor(data);
		}

		private void CheckExceptions(IUpgradeItemData data)
		{
			if (_isCanAddProgress == false)
				throw new InvalidOperationException("Callback still not received");

			if (data.PointLevel >= data.MaxPointLevel)
				throw new InvalidOperationException("Maximum upgrade level");

			int countedMoney = _resourcesProgress.GetDecreasedMoney(data.Price);

			if (countedMoney < 0)
				throw new InvalidOperationException("Not enough money");
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
	}
}