using System;
using Sources.Boot.Scripts.Factories.Presentation.UI;
using Sources.Boot.Scripts.Factories.Presenters;
using Sources.BusinessLogic;
using Sources.BusinessLogic.Repository;
using Sources.BusinessLogic.Services;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Controllers;
using Sources.ControllersInterfaces;
using Sources.ControllersInterfaces.Services;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Services.SceneTriggers;
using Sources.Presentation.UI.Shop;
using Sources.PresentationInterfaces;
using Sources.Utils;

namespace Sources.Boot.Scripts.Factories.Presentation
{
	public class UpgradeWindowPresenterFactory : PresenterFactory<IUpgradeWindowPresenter>, IUpgradeWindowPresenterFactory
	{
		private readonly IAssetLoader _assetLoader;
		private readonly IGameplayInterfacePresenter _gameplayInterfacePresenter;
		private readonly IPersistentProgressService _persistentProgressService;
		private readonly IPlayerModelRepository _playerModelRepository;
		private readonly IPresentersContainerRepository _presentersContainerRepository;
		private readonly IProgressEntityRepository _progressEntityRepository;
		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
		private readonly IResourcesProgressPresenter _resourcesProgressPresenterProvider;
		private readonly ISaveLoader _saveLoader;
		private readonly TranslatorService _translatorService;
		private readonly IUpgradeWindowPresentation _upgradeWindowPresentation;
		private readonly IUpgradeWindowPresenter _upgradeWindowPresenter;

		public UpgradeWindowPresenterFactory(
			IPresentersContainerRepository presentersContainerRepository,
			IAssetLoader assetLoader,
			IProgressSaveLoadDataService progressSaveLoadDataService,
			IPersistentProgressService persistentProgressService,
			IGameplayInterfacePresenter gameplayInterfacePresenter,
			IResourcesProgressPresenter resourcesProgressPresenterProvider,
			TranslatorService localizationService,
			IProgressEntityRepository progressEntityRepository,
			IPlayerModelRepository playerModelRepository,
			ISaveLoader saveLoader
		)
		{
			_presentersContainerRepository = presentersContainerRepository ??
			                                 throw new ArgumentNullException(nameof(presentersContainerRepository));
			_assetLoader
				= assetLoader ?? throw new ArgumentNullException(nameof(assetLoader));
			_progressSaveLoadDataService = progressSaveLoadDataService ??
			                               throw new ArgumentNullException(nameof(progressSaveLoadDataService));
			_persistentProgressService = persistentProgressService ??
			                             throw new ArgumentNullException(nameof(persistentProgressService));
			_gameplayInterfacePresenter = gameplayInterfacePresenter ??
			                              throw new ArgumentNullException(nameof(gameplayInterfacePresenter));
			_resourcesProgressPresenterProvider = resourcesProgressPresenterProvider ??
			                                      throw new ArgumentNullException(
				                                      nameof(resourcesProgressPresenterProvider)
			                                      );
			_translatorService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
			_progressEntityRepository =
				progressEntityRepository ?? throw new ArgumentNullException(nameof(progressEntityRepository));
			_playerModelRepository = playerModelRepository ?? throw new ArgumentNullException(nameof(playerModelRepository));
			_saveLoader = saveLoader ?? throw new ArgumentNullException(nameof(saveLoader));
		}

		private IResourceModelReadOnly GameProgressResourceModelReadOnly =>
			_persistentProgressService.GlobalProgress.ResourceModelReadOnly;

		private int SoftCurrencyCount => GameProgressResourceModelReadOnly.SoftCurrency.Value;

		private string GameObjectsUpgradeTrigger => ResourcesAssetPath.GameObjects.UpgradeTrigger;
		private string UpgradeYesNoButtonsCanvas => ResourcesAssetPath.Scene.UIResources.UpgradeYesNoButtonsCanvas;

		public override IUpgradeWindowPresenter Create()
		{
			IUpgradeTriggerObserver upgradeTrigger = _assetLoader.InstantiateAndGetComponent<UpgradeTriggerObserver>(
				GameObjectsUpgradeTrigger
			);

			UpgradeWindowPresenter presenter = new(
				_upgradeWindowPresentation,
				_progressSaveLoadDataService,
				_gameplayInterfacePresenter,
				_resourcesProgressPresenterProvider
			);

			IUpgradeWindowPresentation upgradeWindowPresentation = new UpgradeWindowViewFactory(
				_presentersContainerRepository,
				_assetLoader,
				_resourcesProgressPresenterProvider,
				_persistentProgressService as IUpdatablePersistentProgressService,
				_translatorService,
				presenter,
				_gameplayInterfacePresenter,
				_progressEntityRepository,
				_playerModelRepository,
				_saveLoader
			).Create();

			var enabler = _assetLoader.InstantiateAndGetComponent<UpgradeWindowActivator>(UpgradeYesNoButtonsCanvas);

			enabler.PhrasesList.Phrases = _translatorService.GetLocalize(enabler.PhrasesList.Phrases);

			enabler.enabled = false;
			enabler.Construct(presenter, upgradeTrigger);
			enabler.enabled = true;

			_upgradeWindowPresentation.Construct(presenter, SoftCurrencyCount, enabler);
			_upgradeWindowPresentation.UpgradeWindowMain.SetActive(false);

			return presenter;
		}
	}
}