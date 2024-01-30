using System;
using System.Collections.Generic;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories.UpgradeShop;
using Sources.Infrastructure.ScriptableObjects.Shop;
using Sources.InfrastructureInterfaces.Upgrade;
using Sources.Presentation.UI.Shop;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs;
using Sources.Utils.Configs.Scripts;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sources.Infrastructure.Factories
{
	public class ShopElementFactory
	{
		private readonly IGameProgress _shopProgress;
		private readonly IAssetProvider _assetProvider;
		private readonly ITranslatorService _translatorService;

		public ShopElementFactory(
			IGameProgress shopProgress,
			IAssetProvider assetProvider,
			ITranslatorService translatorService
		)
		{
			_shopProgress = shopProgress ?? throw new ArgumentNullException(nameof(shopProgress));
			_assetProvider = assetProvider ?? throw new ArgumentNullException(nameof(assetProvider));
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
		}

		public List<UpgradeElementPrefabView> Instantiate(Transform transform)
		{
			List<IUpgradeProgressData> progress = _shopProgress.GetAll();

			UpgradeItemList items
				= _assetProvider.LoadFromResources<UpgradeItemList>(ResourcesAssetPath.Scene.UIResources.ShopItems);
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
			UpgradeElementPrefabView button = Object.Instantiate(
				items.ReadOnlyItems[itemIndex].PrefabView,
				transform.transform
			);

			var title = Localize(items.Items[itemIndex].Title);
			var description = Localize(items.Items[itemIndex].Description);

			button.Construct(
				(IItemChangeable)items.Items[itemIndex],
				items.Items[itemIndex],
				items.ReadOnlyItems[itemIndex],
				title,
				description
			);

			buttons.Add(button);
		}

		private string Localize(string phrase) =>
			_translatorService.Localize(phrase);
	}
}