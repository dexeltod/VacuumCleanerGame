using Sources.Infrastructure.ScriptableObjects.Shop;
using Sources.InfrastructureInterfaces;
using Sources.View.UI.Shop;
using UnityEngine;

namespace Sources.Infrastructure.ScriptableObjects
{
	[CreateAssetMenu(fileName = "Item", menuName = "Data/Shop/Upgrade/Item")]
	public class UpgradeItemPrefab : ProgressItemData, IUpgradeItemPrefab
	{
		[SerializeField] private UpgradeElementPrefab _upgradeElementPrefab;

		[SerializeField] private Sprite _icon;
		public Sprite Icon => _icon;
		public IUpgradeElementConstructable ElementConstructable => _upgradeElementPrefab;
		public UpgradeElementPrefab Prefab => _upgradeElementPrefab;
	}
}