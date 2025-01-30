using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Boot.Scripts.Factories.Presenters;
using Sources.BusinessLogic;
using Sources.BusinessLogic.Configs;
using Sources.BusinessLogic.Repository;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Controllers;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.DomainInterfaces.Models.Shop.Upgrades;
using Sources.Infrastructure.Repository;
using Sources.Infrastructure.Services.SceneTriggers;
using Sources.Presentation.UI.Shop;
using Sources.PresentationInterfaces;
using Sources.PresentationInterfaces.Common;
using Sources.Utils;

namespace Sources.Boot.Scripts.Factories.Presentation
{
	public class UpgradeWindowPresenterFactory : PresenterFactory<IUpgradeWindowPresenter>, IUpgradeWindowPresenterFactory
	{
		private readonly IAssetLoader _assetLoader;
		private readonly IProgressEntityRepository _entityRepository;
		private readonly IView _gameplayInterface;
		private readonly IPersistentProgressService _persistentProgressService;
		private readonly IPlayerModelRepository _playerModelRepository;
		private readonly IResourceModel _resourceModel;
		private readonly ISaveLoader _saveLoader;
		private readonly TranslatorService _translatorService;
		private readonly IUpgradeWindowPresenter _upgradeWindowPresenter;

		public UpgradeWindowPresenterFactory(
			IAssetLoader assetLoader,
			IPersistentProgressService persistentProgressService,
			TranslatorService localizationService,
			IProgressEntityRepository progressEntityRepository,
			IPlayerModelRepository playerModelRepository,
			ISaveLoader saveLoader,
			IView gameplayInterface,
			IResourceModel resourceModel
		)
		{
			_assetLoader
				= assetLoader ?? throw new ArgumentNullException(nameof(assetLoader));
			_persistentProgressService =
				persistentProgressService ?? throw new ArgumentNullException(nameof(persistentProgressService));
			_translatorService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
			_entityRepository =
				progressEntityRepository ?? throw new ArgumentNullException(nameof(progressEntityRepository));
			_playerModelRepository = playerModelRepository ?? throw new ArgumentNullException(nameof(playerModelRepository));
			_saveLoader = saveLoader ?? throw new ArgumentNullException(nameof(saveLoader));
			_gameplayInterface = gameplayInterface ?? throw new ArgumentNullException(nameof(gameplayInterface));
			_resourceModel = resourceModel ?? throw new ArgumentNullException(nameof(resourceModel));
		}

		private IResourceModelReadOnly ResourceModelReadOnly => _persistentProgressService.GlobalProgress.ResourceModel;

		private int SoftCurrencyCount => ResourceModelReadOnly.SoftCurrency.ReadOnlyValue;

		private string GameObjectsUpgradeTrigger => ResourcesAssetPath.GameObjects.UpgradeTrigger;
		private string UpgradeYesNoButtonsCanvas => ResourcesAssetPath.Scene.UIResources.UpgradeYesNoButtonsCanvas;

		private string UIResourcesUpgradeWindow => ResourcesAssetPath.Scene.UIResources.UpgradeWindow;

		public override IUpgradeWindowPresenter Create()
		{
			IUpgradeTriggerObserver upgradeTrigger = InstantiateUpgradeTriggerObserver();
			UpgradeWindowPresentation presentation = LoadPresentation();

			Localize(presentation);

			IReadOnlyList<IUpgradeEntityViewConfig> configs = _entityRepository.GetConfigs();
			IReadOnlyList<IStatUpgradeEntityReadOnly> entities = _entityRepository.GetEntities();

			Dictionary<int, IUpgradeElementPrefabView> views = InstantiateAndConstructViews(configs, entities, presentation);

			UpgradeWindowPresenter presenter = new(
				presentation,
				_gameplayInterface,
				_resourceModel.SoftCurrency,
				views,
				_persistentProgressService,
				_entityRepository,
				_saveLoader,
				new ShopService(
					_entityRepository,
					_playerModelRepository,
					_persistentProgressService.GlobalProgress.ResourceModel
				),
				_entityRepository.GetEntities()
			);

			UpgradeWindowActivator enabler = ConstructUpgradeWindowActivator(presenter, upgradeTrigger);

			presentation.Construct(presenter, SoftCurrencyCount, enabler);
			presentation.UpgradeWindowMain.SetActive(false);

			return presenter;
		}

		private UpgradeWindowActivator ConstructUpgradeWindowActivator(
			UpgradeWindowPresenter presenter,
			IUpgradeTriggerObserver upgradeTrigger)
		{
			var enabler = _assetLoader.InstantiateAndGetComponent<UpgradeWindowActivator>(UpgradeYesNoButtonsCanvas);
			enabler.PhrasesList.Phrases = _translatorService.GetLocalize(enabler.PhrasesList.Phrases);
			enabler.enabled = false;
			enabler.Construct(presenter, upgradeTrigger);
			enabler.enabled = true;
			return enabler;
		}

		private Dictionary<int, IUpgradeElementPrefabView> InstantiateAndConstructViews(
			IReadOnlyList<IUpgradeEntityViewConfig> configs,
			IReadOnlyList<IStatUpgradeEntityReadOnly> entities,
			UpgradeWindowPresentation presentation)
		{
			UpgradeElementPrefabView[] views = configs.Join(
				entities,
				elem => elem.Id,
				elem2 => elem2.ConfigId,
				(elem, elem2) =>
				{
					var view = _assetLoader.InstantiateAndGetComponent<UpgradeElementPrefabView>(
						elem.PrefabView,
						presentation.ContainerTransform
					);

					view.Construct(
						elem.Icon,
						Localize(elem.Title),
						Localize(elem.Description),
						elem.Id,
						elem2.Value,
						_entityRepository.GetPrice(elem.Id),
						elem.MaxProgressCount
					);

					return view;
				}
			).ToArray();

			return views.ToDictionary(elem => elem.ID, elem2 => (IUpgradeElementPrefabView)elem2);
		}

		private UpgradeTriggerObserver InstantiateUpgradeTriggerObserver() =>
			_assetLoader.InstantiateAndGetComponent<UpgradeTriggerObserver>(GameObjectsUpgradeTrigger);

		private UpgradeWindowPresentation LoadPresentation() =>
			_assetLoader.InstantiateAndGetComponent<UpgradeWindowPresentation>(UIResourcesUpgradeWindow);

		private void Localize(IUpgradeWindowPresentation presentation) =>
			presentation.Phrases = _translatorService.GetLocalize(presentation.Phrases);

		private string Localize(string phrase) => _translatorService.GetLocalize(phrase);
	}
}