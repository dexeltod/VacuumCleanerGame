using System;
using Sources.Infrastructure.ScriptableObjects.Shop;
using Sources.Presentation.UI.Shop;
using Sources.ServicesInterfaces.Upgrade;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.Infrastructure.ScriptableObjects
{
	[Serializable] [CreateAssetMenu(fileName = "Item", menuName = "Data/Shop/Upgrade/Item")]
	public class UpgradeItemPrefab : ProgressItemData, IUpgradeItemPrefab
	{
		[FormerlySerializedAs("_upgradeElementPrefab")] [SerializeField]
		private UpgradeElementPrefabView _upgradeElementPrefabView;

		[SerializeField] private Sprite _icon;
		public Sprite Icon => _icon;
		public UpgradeElementPrefabView PrefabView => _upgradeElementPrefabView;
	}
}