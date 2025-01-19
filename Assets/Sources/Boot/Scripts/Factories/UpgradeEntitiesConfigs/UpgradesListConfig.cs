using System.Collections.Generic;
using Sources.InfrastructureInterfaces.Configs;
using UnityEngine;

namespace Sources.Boot.Scripts.Factories.UpgradeEntitiesConfigs
{
	[CreateAssetMenu(fileName = "ShopItemsList", menuName = "Data/Shop/Upgrade/ItemsList")]
	public class UpgradesListConfig : ScriptableObject
	{
		[SerializeField] private PlayerUpgradeShopConfig[] _items;

		public IReadOnlyCollection<PlayerUpgradeShopConfig> ReadOnlyItems => _items;
	}
}
