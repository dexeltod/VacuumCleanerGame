using System.Collections.Generic;
using Sources.Infrastructure.Configs;
using UnityEngine;

namespace Sources.Infrastructure.Factories.UpgradeEntitiesConfigs
{
	[CreateAssetMenu(fileName = "ShopItemsList", menuName = "Data/Shop/Upgrade/ItemsList")]
	public class UpgradeEntityListConfig : ScriptableObject
	{
		[SerializeField] private UpgradeEntityViewConfig[] _items;

		public IReadOnlyCollection<UpgradeEntityViewConfig> ReadOnlyItems => _items;
	}
}