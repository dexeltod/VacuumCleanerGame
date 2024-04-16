using System.Collections.Generic;
using Sources.Infrastructure.Configs;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.Infrastructure.Factories.UpgradeShop
{
	[CreateAssetMenu(fileName = "ShopItems", menuName = "Data/Shop/Upgrade/ItemsList")]
	public class UpgradeEntityListConfig : ScriptableObject
	{
		[FormerlySerializedAs("_list")] [SerializeField]
		private UpgradeEntityViewConfig[] _items;

		public IReadOnlyCollection<UpgradeEntityViewConfig> ReadOnlyItems => _items;
	}
}