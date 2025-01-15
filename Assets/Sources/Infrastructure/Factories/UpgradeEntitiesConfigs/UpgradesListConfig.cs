using System.Collections.Generic;
using Sources.InfrastructureInterfaces.Configs;
using UnityEngine;

namespace Sources.Infrastructure.Factories.UpgradeEntitiesConfigs
{
	[CreateAssetMenu(fileName = "ShopItemsList", menuName = "Data/Shop/Upgrade/ItemsList")]
	public class UpgradesListConfig : ScriptableObject
	{
		[SerializeField] private PlayerUpgradeShopViewsConfig[] _items;

		public IReadOnlyCollection<PlayerUpgradeShopViewsConfig> ReadOnlyItems => _items;
	}
}
