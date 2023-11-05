using System.Collections.Generic;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Factories;
using Sources.InfrastructureInterfaces.DTO;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Upgrade;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.UI;
using Sources.Utils.Configs;
using Sources.View.UI.Shop;
using UnityEngine;

namespace Sources.Application.UI
{
	public class UpgradeWindowFactory : IUpgradeWindowFactory
	{
		private readonly IResourcesProgressPresenter _resourceProgressPresenter;
		private readonly IUpgradeDataFactory _upgradeDataFactory;
		private readonly IAssetProvider _assetProvider;
		private readonly IGameProgressModel _progress;
		private readonly IShopProgressProvider _shopProgressProvider;
		private readonly IPlayerProgressProvider _playerProgressProvider;

		private ShopElementFactory _shopElementFactory;
		private List<UpgradeElementPrefabView> _upgradeElementsPrefabs;

		private GameObject _upgradeWindow;
		public IUpgradeWindow UpgradeWindow { get; private set; }

		public UpgradeWindowFactory
		(
			IAssetProvider assetProvider,
			IUpgradeDataFactory upgradeDataFactory,
			IResourcesProgressPresenter resourceProgressPresenter,
			IGameProgressModel progress,
			IShopProgressProvider shopProgressProvider,
			IPlayerProgressProvider playerProgressProvider
		)
		{
			_assetProvider = assetProvider;
			_upgradeDataFactory = upgradeDataFactory;
			_resourceProgressPresenter = resourceProgressPresenter;
			_progress = progress;
			_shopProgressProvider = shopProgressProvider;
			_playerProgressProvider = playerProgressProvider;
		}

		public GameObject Create()
		{
			if (_upgradeWindow != null)
				return _upgradeWindow;

			Initialize();

			IUpgradeItemData[] items = _upgradeDataFactory.LoadItems();

			ShopPurchaseController shopPurchaseController = new ShopPurchaseController
			(
				UpgradeWindow,
				_upgradeElementsPrefabs,
				_resourceProgressPresenter,
				_shopProgressProvider,
				_playerProgressProvider
			);

			return _upgradeWindow;
		}

		private void Initialize()
		{
			InstantiateWindow();
			ConstructWindow();
			InitButtons();
		}

		private void InstantiateWindow()
		{
			_shopElementFactory ??= new ShopElementFactory(_progress.ShopProgress);
			_upgradeWindow = _assetProvider.Instantiate(ResourcesAssetPath.Scene.UIResources.UpgradeWindow);
		}

		private void InitButtons() =>
			_upgradeElementsPrefabs = _shopElementFactory
				.InstantiateElementPrefabs(UpgradeWindow.ContainerTransform);

		private void ConstructWindow()
		{
			IResourceReadOnly<int> resource = _resourceProgressPresenter.SoftCurrency;

			UpgradeWindow = _upgradeWindow.GetComponent<UpgradeWindow>();
			UpgradeWindow.Construct(resource);
		}
	}
}