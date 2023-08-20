using System.Collections.Generic;
using Sources.Infrastructure.ScriptableObjects;
using Sources.InfrastructureInterfaces;
using Sources.PresentationInterfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.Infrastructure.Factories.UpgradeShop
{
	[CreateAssetMenu(fileName = "ShopItems", menuName = "Data/Shop/Upgrade/ItemsList")]
	public class UpgradeItemList : ScriptableObject, IUpgradeItemList
	{
		[FormerlySerializedAs("_list")] [SerializeField]
		private UpgradeItemPrefabData[] _items;

		public IReadOnlyList<UpgradeItemPrefabData> ReadOnlyItems => _items;
		
		public IUpgradeItemData[] Items => _items;
		public IUpgradeItemPrefabData[] Prefabs => _items;
	}
}