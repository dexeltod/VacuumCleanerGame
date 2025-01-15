using System;
using Sources.BuisenessLogic;
using Sources.BuisenessLogic.ServicesInterfaces;
using Sources.Controllers;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Services.SceneTriggers;
using Sources.Presentation.Factories.UI;
using Sources.Presentation.UI.Shop;
using Sources.PresentationInterfaces;
using Sources.Utils;

namespace Sources.Presentation.Factories
{
	public class UpgradeWindowPresenterFactory : PresenterFactory<UpgradeWindowPresenter>
	{
		private readonly UpgradeWindowViewFactory _upgradeWindowViewFactory;
		private readonly IAssetFactory _assetFactory;
		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
		private readonly IPersistentProgressService _persistentProgressService;
		private readonly IGameplayInterfacePresenter _gameplayInterfacePresenter;
		private readonly IResourcesProgressPresenter _resourcesProgressPresenterProvider;
		private readonly TranslatorService _translatorService;
		private readonly IUpgradeWindowPresenter _upgradeWindowPresenter;

		public UpgradeWindowPresenterFactory(
			UpgradeWindowViewFactory upgradeWindowViewFactory,
			IAssetFactory assetFactory,
			IProgressSaveLoadDataService progressSaveLoadDataService,
			IPersistentProgressService persistentProgressService,
			IGameplayInterfacePresenter gameplayInterfacePresenter,
			IResourcesProgressPresenter resourcesProgressPresenterProvider,
			TranslatorService localizationService
		)
		{
			_upgradeWindowViewFactory
				= upgradeWindowViewFactory ?? throw new ArgumentNullException(nameof(upgradeWindowViewFactory));
			_assetFactory
				= assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
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
		}

		private IResourceModelReadOnly GameProgressResourceModelReadOnly =>
			_persistentProgressService.GlobalProgress.ResourceModelReadOnly;

		private int SoftCurrencyCount => GameProgressResourceModelReadOnly.SoftCurrency.Value;

		private string GameObjectsUpgradeTrigger => ResourcesAssetPath.GameObjects.UpgradeTrigger;
		private string UpgradeYesNoButtonsCanvas => ResourcesAssetPath.Scene.UIResources.UpgradeYesNoButtonsCanvas;

		public override UpgradeWindowPresenter Create()
		{
			IUpgradeWindowPresentation upgradeWindowPresentation = _upgradeWindowViewFactory.Create();

			UpgradeTriggerObserver upgradeTrigger = _assetFactory.InstantiateAndGetComponent<UpgradeTriggerObserver>(
				GameObjectsUpgradeTrigger
			);

			UpgradeWindowPresenter presenter = new(
				upgradeWindowPresentation,
				_progressSaveLoadDataService,
				_gameplayInterfacePresenter,
				_resourcesProgressPresenterProvider
			);

			var enabler = _assetFactory.InstantiateAndGetComponent<UpgradeWindowActivator>(UpgradeYesNoButtonsCanvas);

			enabler.PhrasesList.Phrases = _translatorService.GetLocalize(enabler.PhrasesList.Phrases);

			enabler.enabled = false;
			enabler.Construct(presenter, upgradeTrigger);
			enabler.enabled = true;

			upgradeWindowPresentation.Construct(presenter, SoftCurrencyCount, enabler);
			upgradeWindowPresentation.UpgradeWindowMain.SetActive(false);

			return presenter;
		}
	}
}