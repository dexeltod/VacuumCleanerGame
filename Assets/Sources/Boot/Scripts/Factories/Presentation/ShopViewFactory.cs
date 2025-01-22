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
using Object = UnityEngine.Object;

namespace Sources.Boot.Scripts.Factories.Presentation
{
	public class ShopViewFactory
	{
		private readonly IAssetLoader _assetLoader;
		private readonly IProgressEntityRepository _entityRepository;
		private readonly IGameplayInterfacePresenter _gameplayInterfaceProvider;
		private readonly IPersistentProgressService _persistentProgressServiceProvider;
		private readonly IPlayerModelRepository _playerModelRepositoryProvider;
		private readonly IProgressEntityRepository _progressEntityRepository;
		private readonly IResourcesProgressPresenter _resourcesProgressPresenterProvider;
		private readonly ISaveLoader _saveLoaderProvider;
		private readonly TranslatorService _translatorService;
		private readonly IUpgradeWindowPresenter _upgradeWindowPresenterProvider;

		public ShopViewFactory(
			IPersistentProgressService persistentProgressService,
			TranslatorService translatorService,
			IUpgradeWindowPresenter upgradeWindowPresenterProvider,
			IResourcesProgressPresenter resourcesProgressPresenterProvider,
			IGameplayInterfacePresenter gameplayInterfaceProvider,
			IProgressEntityRepository progressEntityRepository,
			IPlayerModelRepository playerModelRepositoryProvider,
			ISaveLoader saveLoaderProvider,
			IAssetLoader assetLoader,
			IProgressEntityRepository entityRepository
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
			_assetLoader = assetLoader ?? throw new ArgumentNullException(nameof(assetLoader));
			_entityRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
			_persistentProgressServiceProvider = persistentProgressService ??
			                                     throw new ArgumentNullException(nameof(persistentProgressService));
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
		}

		public IEnumerable<IUpgradeElementPrefabView> Create(Transform transform, AudioSource audioSource) =>
			Instantiate(transform, audioSource);

		private UpgradeElementPresenter CreateUpgradeElementPresenter(AudioSource audioSource,
			IReadOnlyList<IUpgradeEntityConfig> configs) =>
			new(
				configs.ToDictionary(
					elem => elem.Id,
					elem2 => (IUpgradeElementChangeableView)elem2.PrefabView.GetComponent<UpgradeElementPrefabView>()
				),
				_persistentProgressServiceProvider,
				_upgradeWindowPresenterProvider,
				_gameplayInterfaceProvider,
				_progressEntityRepository,
				_saveLoaderProvider,
				_resourcesProgressPresenterProvider,
				_playerModelRepositoryProvider,
				_assetLoader.LoadFromResources<AudioClip>(ResourcesAssetPath.SoundNames.SoundBuy),
				_assetLoader.LoadFromResources<AudioClip>(ResourcesAssetPath.SoundNames.SoundClose),
				audioSource
			);

		private IEnumerable<IUpgradeElementPrefabView> Instantiate(
			Component transform,
			AudioSource audioSource
		)
		{
			IReadOnlyList<IUpgradeEntityConfig> configs = _entityRepository.GetConfigs();
			IReadOnlyList<IStatUpgradeEntityReadOnly> entities = _entityRepository.GetEntities();

			UpgradeElementPresenter presenter = CreateUpgradeElementPresenter(audioSource, configs);

			return configs.Join(
				entities,
				elem => elem.Id,
				elem2 => elem2.ConfigId,
				(elem, elem2) =>
				{
					var view = (IUpgradeElementPrefabView)Object.Instantiate(
						elem.PrefabView.GetComponent<UpgradeElementPrefabView>(),
						transform.transform
					);

					view.Construct(
						elem.Icon,
						presenter,
						Localize(elem.Title),
						Localize(elem.Description),
						elem.Id,
						elem2.Value,
						_progressEntityRepository.GetPrice(elem.Id),
						elem.MaxProgressCount
					);

					return view;
				}
			);
		}

		private string Localize(string phrase) =>
			_translatorService.GetLocalize(phrase);
	}
}