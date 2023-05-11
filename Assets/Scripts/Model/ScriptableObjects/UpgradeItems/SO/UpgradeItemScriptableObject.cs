using UnityEngine;

namespace Model.ScriptableObjects.UpgradeItems.SO
{
	[CreateAssetMenu(fileName = "Item", menuName = "Data/Shop/Upgrade/Item")]
	public class UpgradeItemScriptableObject : ScriptableObject
	{
		public enum Upgrade
		{
			Speed,
			Radius,
			SandCount,
		}

		[SerializeField] private UpgradeElement _upgradeElement;

		[SerializeField] private string _title;
		[SerializeField] private string _description;
		[SerializeField] private int _price;
		[SerializeField] private Sprite _icon;
		[SerializeField] private Upgrade _upgradeType;

		public UpgradeElement UpgradeElement => _upgradeElement;
		public Upgrade UpgradeType => _upgradeType;
		public string Title => _title;
		public string Description => _description;
		public int Price => _price;
		public Sprite Icon => _icon;
	}
}