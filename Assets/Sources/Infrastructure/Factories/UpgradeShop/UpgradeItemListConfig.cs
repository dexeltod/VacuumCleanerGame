using System.Collections.Generic;
using Sources.Infrastructure.Configs;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.Infrastructure.Factories.UpgradeShop
{
	[CreateAssetMenu(fileName = "ShopItems", menuName = "Data/Shop/Upgrade/ItemsList")]
	public class UpgradeItemListConfig : ScriptableObject
	{
		[FormerlySerializedAs("_list")] [SerializeField]
		private UpgradeItemViewConfig[] _items;

		public IReadOnlyCollection<UpgradeItemViewConfig> ReadOnlyItems => _items;
	}
}