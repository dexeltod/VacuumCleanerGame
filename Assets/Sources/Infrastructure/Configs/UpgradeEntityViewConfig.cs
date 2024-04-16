using System;
using Sources.Infrastructure.ScriptableObjects.Shop;
using Sources.Presentation.UI.Shop;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.Infrastructure.Configs
{
	[Serializable] [CreateAssetMenu(fileName = "Item", menuName = "Data/Shop/Upgrade/Item")]
	public class UpgradeEntityViewConfig : ProgressItemData
	{
		[FormerlySerializedAs("_upgradeElementPrefab")] [SerializeField]
		private UpgradeElementPrefabView _upgradeElementPrefabView;

		[SerializeField] private Sprite _icon;

		public Sprite Icon => _icon;
		public UpgradeElementPrefabView PrefabView => _upgradeElementPrefabView;
	}
}