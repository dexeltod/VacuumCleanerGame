using System;
using Sources.Infrastructure.ScriptableObjects.Shop;
using Sources.ServicesInterfaces.Upgrade;
using UnityEngine;

namespace Sources.Infrastructure.ScriptableObjects
{
	[Serializable] [CreateAssetMenu(fileName = "Item", menuName = "Data/Shop/Upgrade/Item")]
	public class ShopItem : ProgressItemData, IUpgradeItemPrefab
	{
		public Sprite Icon { get; }
	}
}