using System;
using System.Collections.Generic;
using Sources.Controllers;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories.Presenters;
using Sources.Infrastructure.Providers;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Presenters;
using Sources.Presentation.UI.Shop;
using Sources.PresentationInterfaces;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.DTO;
using Sources.ServicesInterfaces.Upgrade;
using Sources.Utils.Configs.Scripts;
using UnityEngine;
using VContainer;

namespace Sources.Infrastructure.Factories.UI
{
	public class UpgradeWindowViewFactory : IUpgradeWindowViewFactory
	{
		private readonly ResourcesProgressPresenterProvider _resourceProgressPresenterProvider;
		private readonly IProgressUpgradeFactory _progressUpgradeFactory;
		private readonly IAssetFactory _assetFactory;
		private readonly IShopProgressProvider _shopProgressProvider;
		private readonly IPlayerProgressProvider _playerProgressProvider;
		private readonly ITranslatorService _translatorService;
		private readonly GameplayInterfaceProvider _gameplayInterfaceView;
		private readonly IPersistentProgressService _persistentProgressService;
		private readonly ShopElementFactory _shopElementFactory;

		private List<UpgradeElementPrefabView> _upgradeElementsPrefabs;

		private GameObject _upgradeWindowGameObject;
		private IUpgradeWindow _upgradeWindow;

		[Inject]
		public UpgradeWindowViewFactory(
			IAssetFactory assetFactory,
			IProgressUpgradeFactory progressUpgradeFactory,
			ResourcesProgressPresenterProvider resourceProgressPresenter,
			IPersistentProgressService persistentProgressService,
			IShopProgressProvider shopProgressProvider,
			IPlayerProgressProvider playerProgressProvider,
			ITranslatorService translatorService,
			GameplayInterfaceProvider gameplayInterfaceView,
			ShopElementFactory shopElementFactory
		)
		{
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_progressUpgradeFactory = progressUpgradeFactory ??
				throw new ArgumentNullException(nameof(progressUpgradeFactory));
			_resourceProgressPresenterProvider = resourceProgressPresenter ??
				throw new ArgumentNullException(nameof(resourceProgressPresenter));
			_shopProgressProvider
				= shopProgressProvider ?? throw new ArgumentNullException(nameof(shopProgressProvider));
			_playerProgressProvider = playerProgressProvider ??
				throw new ArgumentNullException(nameof(playerProgressProvider));
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
			_gameplayInterfaceView
				= gameplayInterfaceView ?? throw new ArgumentNullException(nameof(gameplayInterfaceView));
			_shopElementFactory = shopElementFactory ?? throw new ArgumentNullException(nameof(shopElementFactory));

			_persistentProgressService = persistentProgressService ??
				throw new ArgumentNullException(nameof(persistentProgressService));
		}

		private string UIResourcesUpgradeWindow => ResourcesAssetPath.Scene.UIResources.UpgradeWindow;

		private Transform UpgradeWindowContainerTransform => _upgradeWindow.ContainerTransform;

		private IGameProgress ShopProgress => _persistentProgressService.GameProgress.ShopProgress;

		private IGameplayInterfaceView GameplayInterface => _gameplayInterfaceView.Implementation;

		private IResourcesProgressPresenter ResourcesProgressPresenter => _resourceProgressPresenterProvider.Implementation;

		private void Localize() =>
			_upgradeWindow.Phrases = _translatorService.Localize(_upgradeWindow.Phrases);

		public IUpgradeWindow Create()
		{
			_upgradeWindow = _assetFactory.InstantiateAndGetComponent<UpgradeWindow>(UIResourcesUpgradeWindow);

			Localize();

			IUpgradeItemData[] items = _progressUpgradeFactory.LoadItems();

			List<UpgradeElementPrefabView> elements = _shopElementFactory.Instantiate(UpgradeWindowContainerTransform);

			ShopPurchasePresenter presenter = new ShopPurchaseControllerFactory(
				_assetFactory,
				new List<IUpgradeElementPrefabView>(elements),
				ResourcesProgressPresenter,
				_shopProgressProvider,
				_playerProgressProvider,
				GameplayInterface
			).Create();

			return _upgradeWindow;
		}
	}
}