using System;
using System.Collections.Generic;
using Sources.BusinessLogic;
using Sources.BusinessLogic.Repository;
using Sources.BusinessLogic.Services;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Controllers;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Services.DomainServices;
using Sources.Presentation.UI.Shop;
using Sources.PresentationInterfaces;
using Sources.Utils;
using UnityEngine;
using VContainer;

namespace Sources.Boot.Scripts.Factories.Presentation.UI
{
	public class UpgradeWindowViewFactory : IUpgradeWindowViewFactory
	{
		private readonly IAssetFactory _assetFactory;
		private readonly IResourcesProgressPresenter _resourceProgressPresenter;
		private readonly IUpdatablePersistentProgressService _updatablePersistentProgressService;
		private readonly TranslatorService _translatorService;
		private readonly IUpgradeWindowPresenter _upgradeWindowPresenter;
		private readonly IGameplayInterfacePresenter _gameplayInterfacePresenter;
		private readonly IProgressEntityRepository _progressEntityRepository;
		private readonly IPlayerModelRepository _playerModelRepository;
		private readonly ISaveLoader _saveLoader;

		private List<UpgradeElementPrefabView> _upgradeElementsPrefabs;

		private GameObject _upgradeWindowGameObject;
		private IUpgradeWindowPresentation _upgradeWindowPresentation;

		public UpgradeWindowViewFactory(IAssetFactory assetFactory,
			IResourcesProgressPresenter resourceProgressPresenter,
			IUpdatablePersistentProgressService updatablePersistentProgressService,
			TranslatorService translatorService,
			IUpgradeWindowPresenter upgradeWindowPresenter,
			IGameplayInterfacePresenter gameplayInterfacePresenter,
			IProgressEntityRepository progressEntityRepository,
			IPlayerModelRepository playerModelRepository,
			ISaveLoader saveLoader)
		{
			_assetFactory
				= assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_resourceProgressPresenter = resourceProgressPresenter;
			_updatablePersistentProgressService = updatablePersistentProgressService;
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
			_upgradeWindowPresenter = upgradeWindowPresenter ?? throw new ArgumentNullException(nameof(upgradeWindowPresenter));
			_gameplayInterfacePresenter =
				gameplayInterfacePresenter ?? throw new ArgumentNullException(nameof(gameplayInterfacePresenter));
			_progressEntityRepository =
				progressEntityRepository ?? throw new ArgumentNullException(nameof(progressEntityRepository));
			_playerModelRepository = playerModelRepository ?? throw new ArgumentNullException(nameof(playerModelRepository));
			_saveLoader = saveLoader ?? throw new ArgumentNullException(nameof(saveLoader));
		}

		private string UIResourcesUpgradeWindow => ResourcesAssetPath.Scene.UIResources.UpgradeWindow;
		private Transform UpgradeWindowContainerTransform => _upgradeWindowPresentation.ContainerTransform;

		public IUpgradeWindowPresentation Create()
		{
			_upgradeWindowPresentation = GetPresentation();

			Localize();

			var shopViewFactory = new ShopViewFactory(
				(IPersistentProgressService)_updatablePersistentProgressService,
				_translatorService,
				_upgradeWindowPresenter,
				_resourceProgressPresenter,
				_gameplayInterfacePresenter,
				_progressEntityRepository,
				_playerModelRepository,
				_saveLoader,
				_assetFactory
			).Create();

			_upgradeWindowPresentation.Construct(_upgradeWindowPresenter);

			_upgradeWindowPresentation.Construct();

			return _upgradeWindowPresentation;
		}

		private UpgradeWindowPresentation GetPresentation() =>
			_assetFactory.InstantiateAndGetComponent<UpgradeWindowPresentation>(UIResourcesUpgradeWindow);

		private void Localize() =>
			_upgradeWindowPresentation.Phrases = _translatorService.GetLocalize(_upgradeWindowPresentation.Phrases);
	}
}
