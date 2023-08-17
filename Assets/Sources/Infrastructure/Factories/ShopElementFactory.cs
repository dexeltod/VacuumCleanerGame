using System.Collections.Generic;
using Sources.Application.Utils.Configs;
using Sources.DIService;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories.UpgradeShop;
using Sources.InfrastructureInterfaces;
using Sources.ServicesInterfaces;
using Sources.View;
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
			List<UpgradeElementPrefab> buttons = new();

			List<IUpgradeProgressData> progress = _shopProgress.GetAll();

			UpgradeItemList items = _assetProvider.Load<UpgradeItemList>(ResourcesAssetPath.ShopConfig.ShopItems);

			InitItems(progress, items);

			Instantiate(transform, items, buttons, progress);
			return buttons;
		}

		private void InitItems(List<IUpgradeProgressData> progress, UpgradeItemList upgradeItems)
		{
			for (var i = 0; i < upgradeItems.Items.Length; i++)
			{
				IUpgradeItemData itemData = upgradeItems.Items[i];
				itemData.SetUpgradeLevel(progress[i].Value);
			}
		}

		private void Instantiate(Transform transform, UpgradeItemList items, List<UpgradeElementPrefab> buttons,
			List<IUpgradeProgressData> progress)
		{
			for (int i = 0; i < progress.Count; i++)
			{
				var button = Object.Instantiate(items.Items[i].UpgradeElementView, transform.transform);
				button.Construct(items.Items[i], progress[i].Value);

				buttons.Add(button);
			}
		}
	}
}