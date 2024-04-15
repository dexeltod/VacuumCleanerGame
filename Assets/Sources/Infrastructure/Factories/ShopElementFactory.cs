using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Controllers;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Configs;
using Sources.Infrastructure.Configs.Scripts;
using Sources.Infrastructure.Factories.UpgradeShop;
using Sources.Infrastructure.Providers;
using Sources.Infrastructure.ScriptableObjects;
using Sources.InfrastructureInterfaces.Providers;
using Sources.Presentation.UI.Shop;
using Sources.PresentationInterfaces;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.Upgrade;
using Sources.Utils;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace Sources.Infrastructure.Factories
{
	public class ShopElementFactory
	{
		private readonly IPersistentProgressServiceProvider _persistentProgressServiceProvider;
		private readonly IAssetFactory _assetFactory;
		private readonly ITranslatorService _translatorService;
		private readonly IPlayerProgressSetterFacadeProvider _playerProgressSetterFacadeProvider;
		private readonly UpgradeWindowPresenterProvider _upgradeWindowPresenterProvider;
		private readonly IResourcesProgressPresenterProvider _resourcesProgressPresenterProvider;
		private readonly IGameplayInterfacePresenterProvider _gameplayInterfaceProvider;

		[Inject]
		public ShopElementFactory(
			IPersistentProgressServiceProvider persistentProgressService,
			IAssetFactory assetFactory,
			ITranslatorService translatorService,
			IPlayerProgressSetterFacadeProvider playerProgressSetterFacadeProvider,
			UpgradeWindowPresenterProvider upgradeWindowPresenterProvider,
			IResourcesProgressPresenterProvider resourcesProgressPresenterProvider,
			IGameplayInterfacePresenterProvider gameplayInterfaceProvider
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
			_persistentProgressServiceProvider = persistentProgressService ??
				throw new ArgumentNullException(nameof(persistentProgressService));
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
			_playerProgressSetterFacadeProvider = playerProgressSetterFacadeProvider ??
				throw new ArgumentNullException(nameof(playerProgressSetterFacadeProvider));
		}

		private string UIResourcesShopItems => ResourcesAssetPath.Scene.UIResources.ShopItems;

		private IGameProgress ShopProgress =>
			_persistentProgressServiceProvider.Implementation.GlobalProgress.ShopProgress;

		private IProgressSetterFacade ProgressSetterFacade => _playerProgressSetterFacadeProvider.Implementation;

		public Dictionary<int, IUpgradeElementPrefabView> Create(Transform transform)
		{
			List<IUpgradeProgressData> progress = ShopProgress.GetAll();

			UpgradeItemListConfig items = LoadItemsList();

			SetUpgradeLevelsToItems(progress, items);

			return Instantiate(transform, items);
		}

		private UpgradeItemListConfig LoadItemsList() =>
			_assetFactory.LoadFromResources<UpgradeItemListConfig>(UIResourcesShopItems);

		private Dictionary<int, IUpgradeElementPrefabView> Instantiate(
			Component transform,
			UpgradeItemListConfig items
		)
		{
			Dictionary<int, IUpgradeItemData> itemsDictionary = new();
			Dictionary<int, IUpgradeElementPrefabView> views = new();
			Dictionary<int, IUpgradeElementChangeableView> changeableViews = new();

			for (int i = 0; i < items.ReadOnlyItems.Count; i++)
			{
				UpgradeItemViewConfig viewConfig = items.ReadOnlyItems.ElementAt(i);
				itemsDictionary.Add(i, viewConfig);
			}

			for (int i = 0; i < items.ReadOnlyItems.Count; i++)
			{
				UpgradeItemViewConfig item = items.ReadOnlyItems.ElementAt(i);

				var view = Object.Instantiate(item.PrefabView, transform.transform);

				views.Add(i, view);
				changeableViews.Add(i, view);
			}

			UpgradeElementPresenter presenter = new(
				ProgressSetterFacade,
				changeableViews,
				itemsDictionary,
				_persistentProgressServiceProvider,
				_upgradeWindowPresenterProvider,
				_gameplayInterfaceProvider
			);

			for (int i = 0; i < views.Count; i++)
			{
				UpgradeItemViewConfig upgradeItemViewConfig = items.ReadOnlyItems.ElementAt(i);

				var translatedTitle = Localize(upgradeItemViewConfig.Title);
				var translatedDescription = Localize(upgradeItemViewConfig.Description);

				views[i].Construct(
					presenter,
					i,
					upgradeItemViewConfig.Icon,
					upgradeItemViewConfig.BoughtPointsCount,
					upgradeItemViewConfig.Price,
					translatedTitle,
					translatedDescription,
					upgradeItemViewConfig.MaxPointLevel
				);
			}

			return views;
		}

		private void SetUpgradeLevelsToItems(List<IUpgradeProgressData> progress, UpgradeItemListConfig config)
		{
			for (var i = 0; i < config.ReadOnlyItems.Count; i++)
				config.ReadOnlyItems.ElementAt(i).SetUpgradeLevel(progress[i].Value);
		}

		private string Localize(string phrase) =>
			_translatorService.GetLocalize(phrase);
	}
}