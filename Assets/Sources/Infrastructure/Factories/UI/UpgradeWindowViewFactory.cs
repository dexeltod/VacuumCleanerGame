using System;
using System.Collections.Generic;
using Sources.ControllersInterfaces;
using Sources.Infrastructure.Configs.Scripts;
using Sources.Infrastructure.Factories.UpgradeShop;
using Sources.Infrastructure.Providers;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Providers;
using Sources.Presentation.UI.Shop;
using Sources.PresentationInterfaces;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using Sources.Utils;
using UnityEngine;
using VContainer;

namespace Sources.Infrastructure.Factories.UI
{
	public class UpgradeWindowViewFactory : IUpgradeWindowViewFactory
	{
		private readonly ResourcesProgressPresenterProvider _resourceProgressPresenterProvider;
		private readonly IAssetFactory _assetFactory;
		private readonly ITranslatorService _translatorService;
		private readonly ShopElementFactory _shopElementFactory;

		private List<UpgradeElementPrefabView> _upgradeElementsPrefabs;

		private GameObject _upgradeWindowGameObject;
		private IUpgradeWindowPresentation _upgradeWindowPresentation;

		[Inject]
		public UpgradeWindowViewFactory(
			IAssetFactory assetFactory,
			ProgressUpgradeFactory progressUpgradeFactory,
			ResourcesProgressPresenterProvider resourceProgressPresenter,
			IPersistentProgressServiceProvider persistentProgressService,
			IShopProgressFacade shopProgressFacade,
			IPlayerProgressSetterFacadeProvider playerProgressSetterFacade,
			ITranslatorService translatorService,
			ShopElementFactory shopElementFactory
		)
		{
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_resourceProgressPresenterProvider = resourceProgressPresenter ??
				throw new ArgumentNullException(nameof(resourceProgressPresenter));
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
			_shopElementFactory = shopElementFactory ?? throw new ArgumentNullException(nameof(shopElementFactory));
		}

		private string UIResourcesUpgradeWindow => ResourcesAssetPath.Scene.UIResources.UpgradeWindow;
		private Transform UpgradeWindowContainerTransform => _upgradeWindowPresentation.ContainerTransform;

		private IResourcesProgressPresenter ResourcesProgressPresenter =>
			_resourceProgressPresenterProvider.Implementation;

		public IUpgradeWindowPresentation Create()
		{
			_upgradeWindowPresentation = GetPresentation();

			Localize();
			_shopElementFactory.Create(UpgradeWindowContainerTransform);

			return _upgradeWindowPresentation;
		}

		private UpgradeWindowPresentation GetPresentation() =>
			_assetFactory.InstantiateAndGetComponent<UpgradeWindowPresentation>(
				UIResourcesUpgradeWindow
			);

		private void Localize() =>
			_upgradeWindowPresentation.Phrases = _translatorService.GetLocalize(_upgradeWindowPresentation.Phrases);
	}
}