using System.Collections.Generic;
using Sources.Infrastructure.Configs;
using UnityEngine;

namespace Sources.Boot.Scripts.UpgradeEntitiesConfigs
{
	[CreateAssetMenu(fileName = "ShopItemsList", menuName = "Data/Shop/Upgrade/ItemsList")]
	public class UpgradesListConfig : ScriptableObject
	{
		[SerializeField] private PlayerUpgradeShopViewConfig[] _items;

		public IReadOnlyCollection<PlayerUpgradeShopViewConfig> ReadOnlyItems => _items;
	}
}