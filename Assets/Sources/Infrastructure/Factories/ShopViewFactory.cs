using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Controllers;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.Models.Shop;
using Sources.DomainInterfaces.Models.Shop.Upgrades;
using Sources.Infrastructure.Configs;
using Sources.Infrastructure.Providers;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Configs;
using Sources.InfrastructureInterfaces.Providers;
using Sources.InfrastructureInterfaces.Repository;
using Sources.Presentation.UI.Shop;
using Sources.PresentationInterfaces;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using Sources.Utils;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace Sources.Infrastructure.Factories
{
	public class ShopViewFactory
	{
		private readonly IPersistentProgressServiceProvider _persistentProgressServiceProvider;
		private readonly ITranslatorService _translatorService;
		private readonly UpgradeWindowPresenterProvider _upgradeWindowPresenterProvider;
		private readonly IResourcesProgressPresenterProvider _resourcesProgressPresenterProvider;
		private readonly IGameplayInterfacePresenterProvider _gameplayInterfaceProvider;
		private readonly IProgressEntityRepositoryProvider _progressEntityRepository;
		private readonly IPlayerModelRepositoryProvider _playerModelRepositoryProvider;
		private readonly ISaveLoaderProvider _saveLoaderProvider;
		private readonly IAssetFactory _assetFactory;

		[Inject]
		public ShopViewFactory(
			IPersistentProgressServiceProvider persistentProgressService,
			ITranslatorService translatorService,
			UpgradeWindowPresenterProvider upgradeWindowPresenterProvider,
			IResourcesProgressPresenterProvider resourcesProgressPresenterProvider,
			IGameplayInterfacePresenterProvider gameplayInterfaceProvider,
			IProgressEntityRepositoryProvider progressEntityRepository,
			IPlayerModelRepositoryProvider playerModelRepositoryProvider,
			ISaveLoaderProvider saveLoaderProvider,
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

		private IProgressEntityRepository ProgressEntityRepository => _progressEntityRepository.Self;

		public Dictionary<int, IUpgradeElementPrefabView> Create(Transform transform, AudioSource audioSource)
		{
			IReadOnlyList<IUpgradeEntityViewConfig> configs = ProgressEntityRepository.GetConfigs();
			IReadOnlyList<IUpgradeEntityReadOnly> entities = ProgressEntityRepository.GetEntities();

			return Instantiate(transform, configs, entities, audioSource);
		}

		private Dictionary<int, IUpgradeElementPrefabView> Instantiate(
			Component transform,
			IReadOnlyList<IUpgradeEntityViewConfig> configs,
			IReadOnlyList<IUpgradeEntityReadOnly> entities,
			AudioSource audioSource
		)
		{
			Dictionary<int, IUpgradeElementPrefabView> views = new();
			Dictionary<int, IUpgradeElementChangeableView> changeableViews = new();

			for (int i = 0; i < configs.Count; i++)
			{
				IUpgradeEntityViewConfig entity = configs.ElementAt(i);

				var view = Object.Instantiate(entity.PrefabView as UpgradeElementPrefabView, transform.transform);

				views.Add(i, view);
				changeableViews.Add(configs.ElementAt(i).Id, view);
			}

			UpgradeElementPresenter presenter = new(
				changeableViews,
				_persistentProgressServiceProvider,
				_upgradeWindowPresenterProvider,
				_gameplayInterfaceProvider,
				ProgressEntityRepository,
				_saveLoaderProvider.Self,
				_resourcesProgressPresenterProvider,
				_playerModelRepositoryProvider,
				_assetFactory.LoadFromResources<AudioClip>(ResourcesAssetPath.SoundNames.SoundBuy),
				_assetFactory.LoadFromResources<AudioClip>(ResourcesAssetPath.SoundNames.SoundClose),
				audioSource
			);

			for (int i = 0; i < views.Count; i++)
			{
				IUpgradeEntityReadOnly upgrade = entities.ElementAt(i);
				IUpgradeEntityViewConfig upgradeEntityViewConfig = configs.ElementAt(i);

				var translatedTitle = Localize(upgradeEntityViewConfig.Title);
				var translatedDescription = Localize(upgradeEntityViewConfig.Description);

				int configId = configs.ElementAt(i).Id;

				views[i].Construct(
					upgradeEntityViewConfig.Icon,
					presenter,
					translatedTitle,
					translatedDescription,
					configId,
					upgrade.Value,
					ProgressEntityRepository.GetPrice(configId),
					upgradeEntityViewConfig.MaxProgressCount
				);
			}

			return views;
		}

		private string Localize(string phrase) =>
			_translatorService.GetLocalize(phrase);
	}
}