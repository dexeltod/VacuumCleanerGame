using System;
using System.Collections;
using System.Collections.Generic;
using Sources.Controllers;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Configs.Scripts;
using Sources.Infrastructure.Factories.UpgradeShop;
using Sources.Infrastructure.Providers;
using Sources.InfrastructureInterfaces.Providers;
using Sources.Presentation.UI.Shop;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.Upgrade;
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

		public List<UpgradeElementPrefabView> Create(Transform transform)
		{
			List<IUpgradeProgressData> progress = ShopProgress.GetAll();

			UpgradeItemListData items = LoadItemsList();

			SetUpgradeLevelsToItems(progress, items);

			return Instantiate(transform, items, progress);
		}

		private UpgradeItemListData LoadItemsList() =>
			_assetFactory.LoadFromResources<UpgradeItemListData>(UIResourcesShopItems);

		private List<UpgradeElementPrefabView> Instantiate(
			Component transform,
			UpgradeItemListData items,
			ICollection progress
		)
		{
			List<UpgradeElementPrefabView> views = new();

			for (int itemIndex = 0; itemIndex < progress.Count; itemIndex++)
				Initialize(transform, items, itemIndex, views);

			return views;
		}

		private void Initialize(
			Component transform,
			UpgradeItemListData items,
			int itemIndex,
			List<UpgradeElementPrefabView> views
		)
		{
			var view = items.ReadOnlyItems[itemIndex].PrefabView;

			UpgradeElementPrefabView upgradeElementPrefabView = Object.Instantiate(
				view,
				transform.transform
			);

			UpgradeElementPresenter presenter = new UpgradeElementPresenter(
				ProgressSetterFacade,
				upgradeElementPrefabView,
				items.Items[itemIndex],
				_persistentProgressServiceProvider,
				_upgradeWindowPresenterProvider,
				_resourcesProgressPresenterProvider,
				_gameplayInterfaceProvider
			);

			var title = Localize(items.Items[itemIndex].Title);
			var description = Localize(items.Items[itemIndex].Description);

			upgradeElementPrefabView.Construct(
				presenter,
				(IItemChangeable)items.Items[itemIndex],
				items.ReadOnlyItems[itemIndex].Icon,
				items.Items[itemIndex].BoughtPointsCount,
				items.Items[itemIndex].Price,
				title,
				description
			);

			views.Add(upgradeElementPrefabView);
		}

		private void SetUpgradeLevelsToItems(List<IUpgradeProgressData> progress, IUpgradeItemListData upgradeItems)
		{
			for (var i = 0; i < upgradeItems.Items.Length; i++)
				upgradeItems.Items[i].SetUpgradeLevel(progress[i].Value);
		}

		private string Localize(string phrase) =>
			_translatorService.GetLocalize(phrase);
	}
}