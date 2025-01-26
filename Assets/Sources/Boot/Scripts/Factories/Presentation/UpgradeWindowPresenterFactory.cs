using System;
using Sources.Boot.Scripts.Factories.Presentation.UI;
using Sources.Boot.Scripts.Factories.Presenters;
using Sources.BusinessLogic;
using Sources.BusinessLogic.Repository;
using Sources.BusinessLogic.Services;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Controllers;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
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
		private readonly IView _gameplayInterface;
		private readonly IPersistentProgressService _persistentProgressService;
		private readonly IPlayerModelRepository _playerModelRepository;
		private readonly IProgressEntityRepository _progressEntityRepository;
		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
		private readonly ISaveLoader _saveLoader;
		private readonly TranslatorService _translatorService;
		private readonly IUpgradeWindowPresenter _upgradeWindowPresenter;

		public UpgradeWindowPresenterFactory(
			IAssetLoader assetLoader,
			IProgressSaveLoadDataService progressSaveLoadDataService,
			IPersistentProgressService persistentProgressService,
			TranslatorService localizationService,
			IProgressEntityRepository progressEntityRepository,
			IPlayerModelRepository playerModelRepository,
			ISaveLoader saveLoader,
			IView gameplayInterface
		)
		{
			_assetLoader
				= assetLoader ?? throw new ArgumentNullException(nameof(assetLoader));
			_progressSaveLoadDataService = progressSaveLoadDataService
			                               ?? throw new ArgumentNullException(nameof(progressSaveLoadDataService));
			_persistentProgressService =
				persistentProgressService ?? throw new ArgumentNullException(nameof(persistentProgressService));
			_translatorService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
			_progressEntityRepository =
				progressEntityRepository ?? throw new ArgumentNullException(nameof(progressEntityRepository));
			_playerModelRepository = playerModelRepository ?? throw new ArgumentNullException(nameof(playerModelRepository));
			_saveLoader = saveLoader ?? throw new ArgumentNullException(nameof(saveLoader));
			_gameplayInterface = gameplayInterface ?? throw new ArgumentNullException(nameof(gameplayInterface));
		}

		private IResourceModelReadOnly ResourceModelReadOnly => _persistentProgressService.GlobalProgress.ResourceModel;

		private int SoftCurrencyCount => ResourceModelReadOnly.SoftCurrency.ReadOnlyValue;

		private string GameObjectsUpgradeTrigger => ResourcesAssetPath.GameObjects.UpgradeTrigger;
		private string UpgradeYesNoButtonsCanvas => ResourcesAssetPath.Scene.UIResources.UpgradeYesNoButtonsCanvas;

		public override IUpgradeWindowPresenter Create()
		{
			IUpgradeTriggerObserver upgradeTrigger = _assetLoader.InstantiateAndGetComponent<UpgradeTriggerObserver>(
				GameObjectsUpgradeTrigger
			);

			IUpgradeWindowPresentation upgradeWindowPresentation = CreateUpgradeWindowPresentation();

			UpgradeWindowPresenter presenter = new(
				upgradeWindowPresentation,
				_progressSaveLoadDataService,
				_gameplayInterface,
				ResourceModelReadOnly.SoftCurrency
			);

			upgradeWindowPresentation.Construct(presenter);

			var enabler = _assetLoader.InstantiateAndGetComponent<UpgradeWindowActivator>(UpgradeYesNoButtonsCanvas);

			enabler.PhrasesList.Phrases = _translatorService.GetLocalize(enabler.PhrasesList.Phrases);

			enabler.enabled = false;
			enabler.Construct(presenter, upgradeTrigger);
			enabler.enabled = true;

			upgradeWindowPresentation.Construct(presenter, SoftCurrencyCount, enabler);
			upgradeWindowPresentation.UpgradeWindowMain.SetActive(false);

			return presenter;
		}

		private IUpgradeWindowPresentation CreateUpgradeWindowPresentation() =>
			new UpgradeWindowViewFactory(
				_assetLoader,
				_persistentProgressService as IUpdatablePersistentProgressService,
				_translatorService,
				_progressEntityRepository,
				_playerModelRepository,
				_saveLoader,
				_persistentProgressService.GlobalProgress.ResourceModel
			).Create();
	}
}