using System;
using System.Collections;
using System.Collections.Generic;
using Sources.Controllers;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories.UpgradeShop;
using Sources.InfrastructureInterfaces.Providers;
using Sources.Presentation.UI.Shop;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.Upgrade;
using Sources.Utils.Configs.Scripts;
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

		[Inject]
		public ShopElementFactory(
			IPersistentProgressServiceProvider persistentProgressService,
			IAssetFactory assetFactory,
			ITranslatorService translatorService,
			IPlayerProgressSetterFacadeProvider playerProgressSetterFacadeProvider
		)
		{
			_playerProgressSetterFacadeProvider = playerProgressSetterFacadeProvider ??
				throw new ArgumentNullException(nameof(playerProgressSetterFacadeProvider));
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

		public List<UpgradeElementPrefabView> Instantiate(Transform transform)
		{
			List<IUpgradeProgressData> progress = ShopProgress.GetAll();

			UpgradeItemList items
				= _assetFactory.LoadFromResources<UpgradeItemList>(UIResourcesShopItems);
			SetUpgradeLevelsToItems(progress, items);

			return Instantiate(transform, items, progress);
		}

		private List<UpgradeElementPrefabView> Instantiate(
			Component transform,
			UpgradeItemList items,
			ICollection progress
		)
		{
			List<UpgradeElementPrefabView> buttons = new();

			for (int i = 0; i < progress.Count; i++)
			{
				UpgradeElementPrefabView button = InstantiatePrefab(transform, items, i);

				UpgradeElementPresenter presenter = new UpgradeElementPresenter(
					ProgressSetterFacade,
					button,
					items.Items[i],
					_persistentProgressServiceProvider
				);

				var title = Localize(items.Items[i].Title);
				var description = Localize(items.Items[i].Description);

				button.Construct(
					presenter,
					(IItemChangeable)items.Items[i],
					items.ReadOnlyItems[i].Icon,
					items.Items[i].BoughtPointsCount,
					items.Items[i].Price,
					title,
					description
				);

				buttons.Add(button);
			}

			return buttons;
		}

		private void SetUpgradeLevelsToItems(List<IUpgradeProgressData> progress, IUpgradeItemList upgradeItems)
		{
			for (var i = 0; i < upgradeItems.Items.Length; i++)
				upgradeItems.Items[i].SetUpgradeLevel(progress[i].Value);
		}

		private UpgradeElementPrefabView InstantiatePrefab(Component transform, UpgradeItemList items, int i) =>
			Object.Instantiate(
				items.ReadOnlyItems[i].PrefabView,
				transform.transform
			);

		private string Localize(string phrase) =>
			_translatorService.Localize(phrase);
	}
}