using System;
using System.Collections.Generic;
using Sources.Controllers;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories.Common.Decorators;
using Sources.Infrastructure.Providers;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Providers;
using Sources.Presentation.UI.Shop;
using Sources.PresentationInterfaces;
using Sources.PresentersInterfaces;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.DTO;
using Sources.ServicesInterfaces.Upgrade;
using Sources.Utils.Configs.Scripts;
using UnityEngine;

namespace Sources.Infrastructure.Factories.UI
{
	public class UpgradeWindowFactory : IUpgradeWindowFactory
	{
		private readonly IResourcesProgressPresenter _resourceProgressPresenter;
		private readonly IProgressUpgradeFactory _progressUpgradeFactory;
		private readonly IAssetResolver _assetResolver;
		private readonly IShopProgressProvider _shopProgressProvider;
		private readonly IPlayerProgressProvider _playerProgressProvider;
		private readonly ITranslatorService _translatorService;
		private readonly GameplayInterfaceProvider _gameplayInterfaceView;
		private readonly IPersistentProgressService _progress;
		private readonly IUpgradeWindow _upgradeWindow;

		private ShopElementFactory _shopElementFactory;
		private List<UpgradeElementPrefabView> _upgradeElementsPrefabs;

		private GameObject _upgradeWindowGameObject;
		private IGameProgress ShopProgress => _progress.GameProgress.ShopProgress;
		private IGameplayInterfaceView GameplayInterface => _gameplayInterfaceView.Instance;

		public UpgradeWindowFactory(
			IAssetResolver assetResolver,
			IProgressUpgradeFactory progressUpgradeFactory,
			IResourcesProgressPresenter resourceProgressPresenter,
			IPersistentProgressService progress,
			IShopProgressProvider shopProgressProvider,
			IPlayerProgressProvider playerProgressProvider,
			ITranslatorService translatorService,
			GameplayInterfaceProvider gameplayInterfaceView,
			IUpgradeWindow upgradeWindow
		)
		{
			_assetResolver = assetResolver ?? throw new ArgumentNullException(nameof(assetResolver));
			_progressUpgradeFactory = progressUpgradeFactory ??
				throw new ArgumentNullException(nameof(progressUpgradeFactory));
			_resourceProgressPresenter = resourceProgressPresenter ??
				throw new ArgumentNullException(nameof(resourceProgressPresenter));
			_shopProgressProvider
				= shopProgressProvider ?? throw new ArgumentNullException(nameof(shopProgressProvider));
			_playerProgressProvider = playerProgressProvider ??
				throw new ArgumentNullException(nameof(playerProgressProvider));
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
			_gameplayInterfaceView
				= gameplayInterfaceView ?? throw new ArgumentNullException(nameof(gameplayInterfaceView));
			_upgradeWindow = upgradeWindow;

			_progress = progress ?? throw new ArgumentNullException(nameof(progress));
		}

		private void Localize() =>
			_upgradeWindow.Phrases = _translatorService.Localize(_upgradeWindow.Phrases);

		private void InitButtons(ShopElementFactory shopElementFactory) =>
			_upgradeElementsPrefabs = shopElementFactory.Instantiate(_upgradeWindow.ContainerTransform);

		public IUpgradeWindow Create()
		{
			ShopElementFactory elementFactory = new ShopElementFactory(
				ShopProgress,
				_assetResolver,
				_translatorService
			);

			Localize();

			InitButtons(elementFactory);

			IUpgradeItemData[] items = _progressUpgradeFactory.LoadItems();

			var presenter = new ShopPurchaseControllerFactory(
				_assetResolver,
				_upgradeElementsPrefabs,
				_resourceProgressPresenter,
				_shopProgressProvider,
				_playerProgressProvider,
				GameplayInterface
			).Create();

			return _upgradeWindow;
		}
	}

	public class ShopPurchaseControllerFactory : PresenterFactory<ShopPurchasePresenter>
	{
		private readonly IAssetResolver _assetResolver;
		private readonly List<UpgradeElementPrefabView> _upgradeElementsPrefabs;
		private readonly IResourcesProgressPresenter _resourceProgressPresenter;
		private readonly IShopProgressProvider _shopProgressProvider;
		private readonly IPlayerProgressProvider _playerProgressProvider;
		private readonly IGameplayInterfaceView _gameplayInterfaceView;

		private string UpgradeWindowPath => ResourcesAssetPath.Scene.UIResources.UpgradeWindow;

		public ShopPurchaseControllerFactory(
			IAssetResolver assetResolver,
			List<UpgradeElementPrefabView> upgradeElementsPrefabs,
			IResourcesProgressPresenter resourceProgressPresenter,
			IShopProgressProvider shopProgressProvider,
			IPlayerProgressProvider playerProgressProvider,
			IGameplayInterfaceView gameplayInterfaceView
		)
		{
			_assetResolver = assetResolver ?? throw new ArgumentNullException(nameof(assetResolver));
			_upgradeElementsPrefabs = upgradeElementsPrefabs ??
				throw new ArgumentNullException(nameof(upgradeElementsPrefabs));
			_resourceProgressPresenter = resourceProgressPresenter ??
				throw new ArgumentNullException(nameof(resourceProgressPresenter));
			_shopProgressProvider
				= shopProgressProvider ?? throw new ArgumentNullException(nameof(shopProgressProvider));
			_playerProgressProvider = playerProgressProvider ??
				throw new ArgumentNullException(nameof(playerProgressProvider));
			_gameplayInterfaceView
				= gameplayInterfaceView ?? throw new ArgumentNullException(nameof(gameplayInterfaceView));
		}

		public override ShopPurchasePresenter Create()
		{
			var gameObject = _assetResolver.Instantiate(UpgradeWindowPath);
			var upgradeWindow = gameObject.GetComponent<IUpgradeWindow>();

			new ShopPurchasePresenter(
				upgradeWindow,
				_upgradeElementsPrefabs,
				_resourceProgressPresenter,
				_shopProgressProvider,
				_playerProgressProvider,
				_gameplayInterfaceView
			);
		}
	}
}