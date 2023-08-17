using Sources.Infrastructure.ScriptableObjects.Shop;
using Sources.PresentationInterfaces;
using Sources.View;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.Infrastructure.ScriptableObjects
{
	[CreateAssetMenu(fileName = "Item", menuName = "Data/Shop/Upgrade/Item")]
	public class UpgradeItemPrefabDataData : ProgressItemData, IUpgradeItemPrefabData
	{
		[SerializeField] private UpgradeElementPrefab _upgradeElementPrefab;

		[SerializeField] private Sprite _icon;
		public Sprite Icon => _icon;
		public IUpgradeElementConstructable UpgradeElementConstructable => _upgradeElementPrefab;
	}
}