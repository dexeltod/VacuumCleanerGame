using System;
using System.Collections.Generic;
using Sources.BusinessLogic;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Controllers;
using Sources.Infrastructure.Services.DomainServices;
using Sources.InfrastructureInterfaces.Factories.Presentations;
using Sources.Presentation.UI.Shop;
using Sources.PresentationInterfaces;
using Sources.Utils;
using UnityEngine;
using VContainer;

namespace Sources.Infrastructure.Factories.Presentation.UI
{
	public class UpgradeWindowViewFactory : IUpgradeWindowViewFactory
	{
		private readonly IAssetFactory _assetFactory;
		private readonly TranslatorService _translatorService;
		private readonly ShopViewFactory _shopViewFactory;

		private List<UpgradeElementPrefabView> _upgradeElementsPrefabs;

		private GameObject _upgradeWindowGameObject;
		private IUpgradeWindowPresentation _upgradeWindowPresentation;

		[Inject]
		public UpgradeWindowViewFactory(
			IAssetFactory assetFactory,
			ResourcesProgressPresenter resourceProgressPresenter,
			PersistentProgressService persistentProgressService,
			TranslatorService translatorService,
			ShopViewFactory shopViewFactory
		)
		{
			_assetFactory
				= assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
			_shopViewFactory = shopViewFactory ?? throw new ArgumentNullException(nameof(shopViewFactory));
		}

		private string UIResourcesUpgradeWindow => ResourcesAssetPath.Scene.UIResources.UpgradeWindow;
		private Transform UpgradeWindowContainerTransform => _upgradeWindowPresentation.ContainerTransform;

		public IUpgradeWindowPresentation Create()
		{
			_upgradeWindowPresentation = GetPresentation();

			Localize();
			_shopViewFactory.Create(UpgradeWindowContainerTransform, _upgradeWindowPresentation.AudioSource);

			return _upgradeWindowPresentation;
		}

		private UpgradeWindowPresentation GetPresentation() =>
			_assetFactory.InstantiateAndGetComponent<UpgradeWindowPresentation>(UIResourcesUpgradeWindow);

		private void Localize() =>
			_upgradeWindowPresentation.Phrases = _translatorService.GetLocalize(_upgradeWindowPresentation.Phrases);
	}
}