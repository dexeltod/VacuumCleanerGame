using System;
using System.Collections.Generic;
using Sources.BusinessLogic;
using Sources.BusinessLogic.Repository;
using Sources.BusinessLogic.Services;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Presentation.UI.Shop;
using Sources.PresentationInterfaces;
using Sources.Utils;
using UnityEngine;

namespace Sources.Boot.Scripts.Factories.Presentation.UI
{
	public class UpgradeWindowViewFactory : IUpgradeWindowViewFactory
	{
		private readonly IAssetLoader _assetLoader;
		private readonly IUpdatablePersistentProgressService _persistentProgressService;
		private readonly IPlayerModelRepository _playerModelRepository;
		private readonly IProgressEntityRepository _progressEntityRepository;
		private readonly IResourceModel _resourceModel;
		private readonly ISaveLoader _saveLoader;
		private readonly TranslatorService _translatorService;

		private GameObject _upgradeWindowGameObject;

		public UpgradeWindowViewFactory(
			IAssetLoader assetLoader,
			IUpdatablePersistentProgressService persistentProgressService,
			TranslatorService translatorService,
			IProgressEntityRepository progressEntityRepository,
			IPlayerModelRepository playerModelRepository,
			ISaveLoader saveLoader,
			IResourceModel resourceModel)
		{
			_assetLoader
				= assetLoader ?? throw new ArgumentNullException(nameof(assetLoader));
			_persistentProgressService = persistentProgressService;
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));

			_progressEntityRepository =
				progressEntityRepository ?? throw new ArgumentNullException(nameof(progressEntityRepository));
			_playerModelRepository = playerModelRepository ?? throw new ArgumentNullException(nameof(playerModelRepository));
			_saveLoader = saveLoader ?? throw new ArgumentNullException(nameof(saveLoader));
			_resourceModel = resourceModel ?? throw new ArgumentNullException(nameof(resourceModel));
		}

		private string UIResourcesUpgradeWindow => ResourcesAssetPath.Scene.UIResources.UpgradeWindow;

		public IUpgradeWindowPresentation Create()
		{
			UpgradeWindowPresentation presentation = LoadPresentation();
			Transform transform = presentation.ContainerTransform;

			Localize(presentation);

			IEnumerable<IUpgradeElementPrefabView> upgradeElementPrefabViews = new ShopViewFactory(
				_persistentProgressService,
				_translatorService,
				_progressEntityRepository,
				_playerModelRepository,
				_saveLoader,
				_assetLoader,
				_progressEntityRepository,
				_resourceModel
			).Create(transform, presentation.AudioSource);



			return presentation;
		}

		private UpgradeWindowPresentation LoadPresentation() =>
			_assetLoader.InstantiateAndGetComponent<UpgradeWindowPresentation>(UIResourcesUpgradeWindow);

		private void Localize(UpgradeWindowPresentation presentation) =>
			presentation.Phrases = _translatorService.GetLocalize(presentation.Phrases);
	}
}
