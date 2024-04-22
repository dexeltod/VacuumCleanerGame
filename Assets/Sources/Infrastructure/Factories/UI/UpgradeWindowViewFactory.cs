using System;
using System.Collections.Generic;
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
		private readonly IAssetFactory _assetFactory;
		private readonly ITranslatorService _translatorService;
		private readonly ShopViewFactory _shopViewFactory;

		private List<UpgradeElementPrefabView> _upgradeElementsPrefabs;

		private GameObject _upgradeWindowGameObject;
		private IUpgradeWindowPresentation _upgradeWindowPresentation;

		[Inject]
		public UpgradeWindowViewFactory(
			IAssetFactory assetFactory,
			ResourcesProgressPresenterProvider resourceProgressPresenter,
			IPersistentProgressServiceProvider persistentProgressService,
			IPlayerProgressSetterFacadeProvider playerProgressSetterFacade,
			ITranslatorService translatorService,
			ShopViewFactory shopViewFactory
		)
		{
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
			_shopViewFactory = shopViewFactory ?? throw new ArgumentNullException(nameof(shopViewFactory));
		}

		private string UIResourcesUpgradeWindow => ResourcesAssetPath.Scene.UIResources.UpgradeWindow;
		private Transform UpgradeWindowContainerTransform => _upgradeWindowPresentation.ContainerTransform;

		public IUpgradeWindowPresentation Create()
		{
			_upgradeWindowPresentation = GetPresentation();

			Localize();
			_shopViewFactory.Create(UpgradeWindowContainerTransform);

			return _upgradeWindowPresentation;
		}

		private UpgradeWindowPresentation GetPresentation() =>
			_assetFactory.InstantiateAndGetComponent<UpgradeWindowPresentation>(UIResourcesUpgradeWindow);

		private void Localize() =>
			_upgradeWindowPresentation.Phrases = _translatorService.GetLocalize(_upgradeWindowPresentation.Phrases);
	}
}