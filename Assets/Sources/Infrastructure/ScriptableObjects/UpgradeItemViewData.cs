using Sources.Infrastructure.ScriptableObjects.Shop;
using Sources.PresetrationInterfaces;
using Sources.View;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.Infrastructure.ScriptableObjects
{
	[CreateAssetMenu(fileName = "Item", menuName = "Data/Shop/Upgrade/Item")]
	public class UpgradeItemViewData : ProgressItem, IUpgradeItemView
	{
		[FormerlySerializedAs("_upgradeElement")] [SerializeField]
		private UpgradeElementView _upgradeElementView;

		[SerializeField] private Sprite _icon;
		public Sprite Icon => _icon;
		public IUpgradeElementView UpgradeElementView => _upgradeElementView;
		
		
	}
}