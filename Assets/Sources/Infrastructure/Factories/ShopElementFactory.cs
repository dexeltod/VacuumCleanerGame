using System.Collections.Generic;
using Sources.Application.Utils.Configs;
using Sources.DIService;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories.UpgradeShop;
using Sources.InfrastructureInterfaces.Upgrade;
using Sources.ServicesInterfaces;
using Sources.View.UI.Shop;
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
			_assetProvider = GameServices.Container.Get<IAssetProvider>();
		}

		public List<UpgradeElementPrefab> InstantiateElementPrefabs(Transform transform)
		{
			List<IUpgradeProgressData> progress = _shopProgress.GetAll();

			UpgradeItemList items = _assetProvider.Load<UpgradeItemList>(ResourcesAssetPath.GameObjects.ShopItems);
			SetUpgradeLevelsToItems(progress, items);

			return Instantiate(transform, items, progress);
		}

		private void InstantiateButtons(Transform transform, UpgradeItemList items, List<UpgradeElementPrefab> buttons,
			int itemIndex)
		{
			UpgradeElementPrefab button =
				Object.Instantiate(items.ReadOnlyItems[itemIndex].Prefab, transform.transform);

			button.Construct(items.Items[itemIndex], items.ReadOnlyItems[itemIndex]);

			buttons.Add(button);
		}

		private void SetUpgradeLevelsToItems(List<IUpgradeProgressData> progress, IUpgradeItemList upgradeItems)
		{
			for (var i = 0; i < upgradeItems.Items.Length; i++)
				upgradeItems.Items[i].SetUpgradeLevel(progress[i].Value);
		}

		private List<UpgradeElementPrefab> Instantiate(Transform transform, UpgradeItemList items,
			List<IUpgradeProgressData> progress)
		{
			List<UpgradeElementPrefab> buttons = new();
			InitButtons(transform, items, progress, buttons);

			return buttons;
		}

		private void InitButtons(Transform transform, UpgradeItemList items, List<IUpgradeProgressData> progress,
			List<UpgradeElementPrefab> buttons)
		{
			for (int i = 0; i < progress.Count; i++)
				InstantiateButtons(transform, items, buttons, i);
		}
	}
}