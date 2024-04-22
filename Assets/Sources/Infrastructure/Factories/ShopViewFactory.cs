using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Controllers;
using Sources.Domain.Temp;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Configs;
using Sources.Infrastructure.Providers;
using Sources.Infrastructure.Repositories;
using Sources.InfrastructureInterfaces.Configs;
using Sources.InfrastructureInterfaces.Providers;
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
		private readonly IAssetFactory _assetFactory;
		private readonly ITranslatorService _translatorService;
		private readonly IPlayerProgressSetterFacadeProvider _playerProgressSetterFacadeProvider;
		private readonly UpgradeWindowPresenterProvider _upgradeWindowPresenterProvider;
		private readonly IResourcesProgressPresenterProvider _resourcesProgressPresenterProvider;
		private readonly IGameplayInterfacePresenterProvider _gameplayInterfaceProvider;
		private readonly UpgradeProgressRepositoryProvider _upgradeProgressRepositoryProvider;
		private readonly IProgressService _progressService;

		[Inject]
		public ShopViewFactory(
			IPersistentProgressServiceProvider persistentProgressService,
			IAssetFactory assetFactory,
			ITranslatorService translatorService,
			IPlayerProgressSetterFacadeProvider playerProgressSetterFacadeProvider,
			UpgradeWindowPresenterProvider upgradeWindowPresenterProvider,
			IResourcesProgressPresenterProvider resourcesProgressPresenterProvider,
			IGameplayInterfacePresenterProvider gameplayInterfaceProvider,
			UpgradeProgressRepositoryProvider upgradeProgressRepositoryProvider,
			IProgressService progressService
		)
		{
			_playerProgressSetterFacadeProvider = playerProgressSetterFacadeProvider ??
				throw new ArgumentNullException(nameof(playerProgressSetterFacadeProvider));
			_upgradeWindowPresenterProvider = upgradeWindowPresenterProvider ??
				throw new ArgumentNullException(nameof(upgradeWindowPresenterProvider));
			_resourcesProgressPresenterProvider = resourcesProgressPresenterProvider ??
				throw new ArgumentNullException(nameof(resourcesProgressPresenterProvider));
			_gameplayInterfaceProvider = gameplayInterfaceProvider ??
				throw new ArgumentNullException(nameof(gameplayInterfaceProvider));
			_upgradeProgressRepositoryProvider = upgradeProgressRepositoryProvider ??
				throw new ArgumentNullException(nameof(upgradeProgressRepositoryProvider));
			_progressService = progressService ?? throw new ArgumentNullException(nameof(progressService));
			_persistentProgressServiceProvider = persistentProgressService ??
				throw new ArgumentNullException(nameof(persistentProgressService));
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
			_playerProgressSetterFacadeProvider = playerProgressSetterFacadeProvider ??
				throw new ArgumentNullException(nameof(playerProgressSetterFacadeProvider));
		}

		private string UIResourcesShopItems => ResourcesAssetPath.Scene.UIResources.ShopItems;

		private IShopEntity ShopProgress => _persistentProgressServiceProvider.Implementation.GlobalProgress.ShopEntity;

		private IUpgradeProgressRepository UpgradeProgressRepository =>
			_upgradeProgressRepositoryProvider.Implementation;

		private IProgressSetterFacade ProgressSetterFacade => _playerProgressSetterFacadeProvider.Implementation;

		public Dictionary<int, IUpgradeElementPrefabView> Create(Transform transform)
		{
			IReadOnlyList<IUpgradeEntityViewConfig> configs = _progressService.GetConfigs();
			IReadOnlyList<IProgressEntity> entities = _progressService.GetEntities();

			return Instantiate(transform, configs, entities);
		}

		private Dictionary<int, IUpgradeElementPrefabView> Instantiate(
			Component transform,
			IReadOnlyList<IUpgradeEntityViewConfig> configs,
			IReadOnlyList<IProgressEntity> entities
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
				ProgressSetterFacade,
				changeableViews,
				_persistentProgressServiceProvider,
				_upgradeWindowPresenterProvider,
				_gameplayInterfaceProvider,
				UpgradeProgressRepository
			);

			for (int i = 0; i < views.Count; i++)
			{
				IProgressEntity progress = entities.ElementAt(i);
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
					progress.CurrentLevel,
					_progressService.GetPrice(configId),
					upgradeEntityViewConfig.MaxProgressCount
				);
			}

			return views;
		}

		private string Localize(string phrase) =>
			_translatorService.GetLocalize(phrase);
	}
}