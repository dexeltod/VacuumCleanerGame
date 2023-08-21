using System.Collections.Generic;
using Sources.Application.Utils.Configs;
using Sources.DIService;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories.UpgradeShop;
using Sources.InfrastructureInterfaces;
using Sources.ServicesInterfaces;
using Sources.View.UI.Shop;
using UnityEngine;

namespace Sources.Infrastructure.Factories
{
	public class ShopElementFactory
	{
		private readonly IGameProgress _shopProgress;
		private readonly IResourceProvider _assetProvider;

		public ShopElementFactory(IGameProgress shopProgress)
		{
			_shopProgress = shopProgress;
			_assetProvider = GameServices.Container.Get<IResourceProvider>();
		}

		public List<UpgradeElementPrefab> InstantiateElementPrefabs(Transform transform)
		{
			List<IUpgradeProgressData> progress = _shopProgress.GetAll();

			UpgradeItemList items = _assetProvider.Load<UpgradeItemList>(ResourcesAssetPath.Configs.ShopItems);
			InitItems(progress, items);

			return Instantiate(transform, items, progress);
		}

		private void InitItems(List<IUpgradeProgressData> progress, IUpgradeItemList upgradeItems)
		{
			for (var i = 0; i < upgradeItems.Items.Length; i++)
			{
				IUpgradeItemData itemData = upgradeItems.Items[i];
				itemData.SetUpgradeLevel(progress[i].Value);
			}
		}

		private List<UpgradeElementPrefab> Instantiate(Transform transform, UpgradeItemList items,
			List<IUpgradeProgressData> progress)
		{
			List<UpgradeElementPrefab> buttons = new();
			InitButtons(transform, items, progress, buttons);
			return buttons;
		}

		private static void InitButtons(Transform transform, UpgradeItemList items, List<IUpgradeProgressData> progress,
			List<UpgradeElementPrefab> buttons)
		{
			for (int i = 0; i < progress.Count; i++)
			{
				var button = Object.Instantiate(items.ReadOnlyItems[i].Prefab, transform.transform);
				button.Construct(items.Items[i], items.ReadOnlyItems[i]);

				buttons.Add(button);
			}
		}
	}
}