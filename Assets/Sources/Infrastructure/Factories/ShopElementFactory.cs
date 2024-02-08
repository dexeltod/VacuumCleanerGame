using System;
using System.Collections.Generic;
using Sources.Controllers;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories.UpgradeShop;
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
		private readonly IPersistentProgressService _persistentProgressService;
		private readonly IAssetFactory _assetFactory;
		private readonly ITranslatorService _translatorService;

		private string UIResourcesShopItems => ResourcesAssetPath.Scene.UIResources.ShopItems;
		private IGameProgress ShopProgress => _persistentProgressService.GameProgress.ShopProgress;

		[Inject]
		public ShopElementFactory(
			IPersistentProgressService persistentProgressService,
			IAssetFactory assetFactory,
			ITranslatorService translatorService
		)
		{
			_persistentProgressService = persistentProgressService ??
				throw new ArgumentNullException(nameof(persistentProgressService));
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
		}

		public List<UpgradeElementPrefabView> Instantiate(Transform transform)
		{
			List<IUpgradeProgressData> progress = ShopProgress.GetAll();

			UpgradeItemList items
				= _assetFactory.LoadFromResources<UpgradeItemList>(UIResourcesShopItems);
			SetUpgradeLevelsToItems(progress, items);

			return Instantiate(transform, items, progress);
		}

		private void SetUpgradeLevelsToItems(List<IUpgradeProgressData> progress, IUpgradeItemList upgradeItems)
		{
			for (var i = 0; i < upgradeItems.Items.Length; i++)
				upgradeItems.Items[i].SetUpgradeLevel(progress[i].Value);
		}

		private List<UpgradeElementPrefabView> Instantiate(
			Transform transform,
			UpgradeItemList items,
			List<IUpgradeProgressData> progress
		)
		{
			List<UpgradeElementPrefabView> buttons = new();
			InitButtons(transform, items, progress, buttons);

			return buttons;
		}

		private void InitButtons(
			Transform transform,
			UpgradeItemList items,
			List<IUpgradeProgressData> progress,
			List<UpgradeElementPrefabView> buttons
		)
		{
			for (int i = 0; i < progress.Count; i++)
				Construct(transform, items, buttons, i);
		}

		private void Construct(
			Transform transform,
			UpgradeItemList items,
			List<UpgradeElementPrefabView> buttons,
			int itemIndex
		)
		{
			var presenter = new UpgradeElementPresenter();

			UpgradeElementPrefabView button = Object.Instantiate(
				items.ReadOnlyItems[itemIndex].PrefabView,
				transform.transform
			);

			var title = Localize(items.Items[itemIndex].Title);
			var description = Localize(items.Items[itemIndex].Description);

			button.Construct(
				presenter,
				(IItemChangeable)items.Items[itemIndex],
				items.Items[itemIndex],
				items.ReadOnlyItems[itemIndex],
				title,
				description,
				items.Items[itemIndex].IdName
			);

			buttons.Add(button);
		}

		private string Localize(string phrase) =>
			_translatorService.Localize(phrase);
	}
}