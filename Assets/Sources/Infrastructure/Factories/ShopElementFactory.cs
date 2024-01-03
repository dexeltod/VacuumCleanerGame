using System.Collections.Generic;
using Sources.DIService;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories.UpgradeShop;
using Sources.InfrastructureInterfaces.Upgrade;
using Sources.Presentation.UI.Shop;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs;
using Sources.Utils.Configs.Scripts;
using UnityEngine;

namespace Sources.Infrastructure.Factories
{
	public class ShopElementFactory
	{
		private readonly IGameProgress _shopProgress;
		private readonly IAssetProvider _assetProvider;

		public ShopElementFactory(IGameProgress shopProgress)
		{
			_shopProgress = shopProgress;
			_assetProvider = ServiceLocator.Container.Get<IAssetProvider>();
		}

		public List<UpgradeElementPrefabView> InstantiateElementPrefabs(Transform transform)
		{
			List<IUpgradeProgressData> progress = _shopProgress.GetAll();

			UpgradeItemList items
				= _assetProvider.LoadFromResources<UpgradeItemList>(ResourcesAssetPath.GameObjects.ShopItems);
			SetUpgradeLevelsToItems(progress, items);

			return Instantiate(transform, items, progress);
		}

		private void InstantiateButtons(
			Transform transform,
			UpgradeItemList items,
			List<UpgradeElementPrefabView> buttons,
			int itemIndex
		)
		{
			UpgradeElementPrefabView button =
				Object.Instantiate(
					items.ReadOnlyItems[itemIndex].PrefabView,
					transform.transform
				);

			button.Construct(items.Items[itemIndex], items.ReadOnlyItems[itemIndex]);

			buttons.Add(button);
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
				InstantiateButtons(transform, items, buttons, i);
		}
	}
}