using System;
using System.Collections.Generic;
using System.Linq;
using Sources.BusinessLogic;
using Sources.BusinessLogic.Repository;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Controllers;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.DomainInterfaces.Models.Shop.Upgrades;
using Sources.InfrastructureInterfaces.Configs;
using Sources.Presentation.UI.Shop;
using Sources.PresentationInterfaces;
using Sources.Utils;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace Sources.Boot.Scripts.Factories.Presentation
{
	public class ShopViewFactory
	{
		private readonly IPersistentProgressService _persistentProgressServiceProvider;
		private readonly TranslatorService _translatorService;
		private readonly UpgradeWindowPresenter _upgradeWindowPresenterProvider;
		private readonly IResourcesProgressPresenter _resourcesProgressPresenterProvider;
		private readonly IGameplayInterfacePresenter _gameplayInterfaceProvider;
		private readonly IProgressEntityRepository _progressEntityRepository;
		private readonly IPlayerModelRepository _playerModelRepositoryProvider;
		private readonly ISaveLoader _saveLoaderProvider;
		private readonly IAssetFactory _assetFactory;

		[Inject]
		public ShopViewFactory(
			IPersistentProgressService persistentProgressService,
			TranslatorService translatorService,
			UpgradeWindowPresenter upgradeWindowPresenterProvider,
			IResourcesProgressPresenter resourcesProgressPresenterProvider,
			IGameplayInterfacePresenter gameplayInterfaceProvider,
			IProgressEntityRepository progressEntityRepository,
			IPlayerModelRepository playerModelRepositoryProvider,
			ISaveLoader saveLoaderProvider,
			IAssetFactory assetFactory
		)
		{
			_upgradeWindowPresenterProvider = upgradeWindowPresenterProvider ??
			                                  throw new ArgumentNullException(nameof(upgradeWindowPresenterProvider));
			_resourcesProgressPresenterProvider = resourcesProgressPresenterProvider ??
			                                      throw new ArgumentNullException(nameof(resourcesProgressPresenterProvider));
			_gameplayInterfaceProvider = gameplayInterfaceProvider ??
			                             throw new ArgumentNullException(nameof(gameplayInterfaceProvider));
			_progressEntityRepository = progressEntityRepository ??
			                            throw new ArgumentNullException(nameof(progressEntityRepository));
			_playerModelRepositoryProvider = playerModelRepositoryProvider ??
			                                 throw new ArgumentNullException(nameof(playerModelRepositoryProvider));
			_saveLoaderProvider = saveLoaderProvider ?? throw new ArgumentNullException(nameof(saveLoaderProvider));
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_persistentProgressServiceProvider = persistentProgressService ??
			                                     throw new ArgumentNullException(nameof(persistentProgressService));
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
		}

		public Dictionary<int, IUpgradeElementPrefabView> Create(Transform transform, AudioSource audioSource)
		{
			IReadOnlyList<IUpgradeEntityConfig> configs = _progressEntityRepository.GetConfigs();
			IReadOnlyList<IStatUpgradeEntityReadOnly> entities = _progressEntityRepository.GetEntities();

			return Instantiate(transform, configs, entities, audioSource);
		}

		private Dictionary<int, IUpgradeElementPrefabView> Instantiate(
			Component transform,
			IReadOnlyList<IUpgradeEntityConfig> configs,
			IReadOnlyList<IStatUpgradeEntityReadOnly> entities,
			AudioSource audioSource
		)
		{
			Dictionary<int, IUpgradeElementPrefabView> views = new();
			Dictionary<int, IUpgradeElementChangeableView> changeableViews = new();

			for (int i = 0; i < configs.Count; i++)
			{
				IUpgradeEntityConfig entity = configs.ElementAt(i);

				var view = Object.Instantiate(entity.PrefabView.GetComponent<UpgradeElementPrefabView>(), transform.transform);

				views.Add(i, view);
				changeableViews.Add(configs.ElementAt(i).Id, view);
			}

			UpgradeElementPresenter presenter = new(
				changeableViews,
				_persistentProgressServiceProvider,
				_upgradeWindowPresenterProvider,
				_gameplayInterfaceProvider,
				_progressEntityRepository,
				_saveLoaderProvider,
				_resourcesProgressPresenterProvider,
				_playerModelRepositoryProvider,
				_assetFactory.LoadFromResources<AudioClip>(ResourcesAssetPath.SoundNames.SoundBuy),
				_assetFactory.LoadFromResources<AudioClip>(ResourcesAssetPath.SoundNames.SoundClose),
				audioSource
			);

			for (int i = 0; i < views.Count; i++)
			{
				IStatUpgradeEntityReadOnly statUpgrade = entities.ElementAt(i);
				IUpgradeEntityConfig upgradeEntityConfig = configs.ElementAt(i);

				var translatedTitle = Localize(upgradeEntityConfig.Title);
				var translatedDescription = Localize(upgradeEntityConfig.Description);

				int configId = configs.ElementAt(i).Id;

				views[i].Construct(
					upgradeEntityConfig.Icon,
					presenter,
					translatedTitle,
					translatedDescription,
					configId,
					statUpgrade.Value,
					_progressEntityRepository.GetPrice(configId),
					upgradeEntityConfig.MaxProgressCount
				);
			}

			return views;
		}

		private string Localize(string phrase) =>
			_translatorService.GetLocalize(phrase);
	}
}