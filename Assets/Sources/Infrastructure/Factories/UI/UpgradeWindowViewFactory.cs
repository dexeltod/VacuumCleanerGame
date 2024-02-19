using System;
using System.Collections.Generic;
using Sources.ControllersInterfaces;
using Sources.Infrastructure.Providers;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Providers;
using Sources.Presentation.UI.Shop;
using Sources.PresentationInterfaces;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
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
		private readonly IShopProgressFacade _shopProgressFacade;
		private readonly IPlayerProgressSetterFacadeProvider _playerProgressSetterFacade;
		private readonly ITranslatorService _translatorService;
		private readonly GameplayInterfaceProvider _gameplayInterfaceView;
		private readonly IPersistentProgressServiceProvider _persistentProgressService;
		private readonly ShopElementFactory _shopElementFactory;

		private List<UpgradeElementPrefabView> _upgradeElementsPrefabs;

		private GameObject _upgradeWindowGameObject;
		private IUpgradeWindow _upgradeWindow;

		[Inject]
		public UpgradeWindowViewFactory(
			IAssetFactory assetFactory,
			IProgressUpgradeFactory progressUpgradeFactory,
			ResourcesProgressPresenterProvider resourceProgressPresenter,
			IPersistentProgressServiceProvider persistentProgressService,
			IShopProgressFacade shopProgressFacade,
			IPlayerProgressSetterFacadeProvider playerProgressSetterFacade,
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
			_shopProgressFacade
				= shopProgressFacade ?? throw new ArgumentNullException(nameof(shopProgressFacade));
			_playerProgressSetterFacade = playerProgressSetterFacade ??
				throw new ArgumentNullException(nameof(playerProgressSetterFacade));
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
			_gameplayInterfaceView
				= gameplayInterfaceView ?? throw new ArgumentNullException(nameof(gameplayInterfaceView));
			_shopElementFactory = shopElementFactory ?? throw new ArgumentNullException(nameof(shopElementFactory));

			_persistentProgressService = persistentProgressService ??
				throw new ArgumentNullException(nameof(persistentProgressService));
		}

		private string UIResourcesUpgradeWindow => ResourcesAssetPath.Scene.UIResources.UpgradeWindow;
		private Transform UpgradeWindowContainerTransform => _upgradeWindow.ContainerTransform;
		private IGameplayInterfaceView GameplayInterface => _gameplayInterfaceView.Implementation;

		private IResourcesProgressPresenter ResourcesProgressPresenter =>
			_resourceProgressPresenterProvider.Implementation;

		public IUpgradeWindow Create()
		{
			_upgradeWindow = _assetFactory.InstantiateAndGetComponent<UpgradeWindow>(UIResourcesUpgradeWindow);

			Localize();
			_shopElementFactory.Instantiate(UpgradeWindowContainerTransform);

			// new ShopPurchaseControllerFactory(
			// 	_assetFactory,
			// 	new List<IUpgradeElementPrefabView>(elements),
			// 	ResourcesProgressPresenter,
			// 	_shopProgressFacade,
			// 	_playerProgressSetterFacade,
			// 	GameplayInterface
			// ).Create();

			return _upgradeWindow;
		}

		private void Localize() =>
			_upgradeWindow.Phrases = _translatorService.Localize(_upgradeWindow.Phrases);
	}
}