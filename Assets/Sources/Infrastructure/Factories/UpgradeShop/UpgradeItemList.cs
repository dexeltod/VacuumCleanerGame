using System.Collections.Generic;
using Sources.Infrastructure.ScriptableObjects;
using Sources.ServicesInterfaces.Upgrade;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.Infrastructure.Factories.UpgradeShop
{
	[CreateAssetMenu(fileName = "ShopItems", menuName = "Data/Shop/Upgrade/ItemsList")]
	public class UpgradeItemList : ScriptableObject, IUpgradeItemList
	{
		[FormerlySerializedAs("_list")] [SerializeField]
		private UpgradeItemPrefab[] _items;

		public IReadOnlyList<UpgradeItemPrefab> ReadOnlyItems => _items;
		public IUpgradeItemData[] Items => _items;
		public IUpgradeItemPrefab[] Prefabs => _items;
	}
}