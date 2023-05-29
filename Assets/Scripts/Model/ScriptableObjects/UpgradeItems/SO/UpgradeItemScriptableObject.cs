using System;
using UnityEngine;
using UnityEngine.Serialization;
using View.UI.Shop;

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

		[FormerlySerializedAs("_upgradeElement")] [SerializeField]
		private UpgradeElementView _upgradeElementView;

		[SerializeField] private string _title;
		[SerializeField] private string _description;
		[SerializeField] private int _price;
		[SerializeField] private Sprite _icon;
		[SerializeField] private Upgrade _upgradeType;

		private int _currentUpgradePoint;
		private bool _isConstructed = false;

		public UpgradeElementView UpgradeElementView => _upgradeElementView;
		public Upgrade UpgradeType => _upgradeType;
		public string Title => _title;
		public string Description => _description;
		public int Price => _price;
		public Sprite Icon => _icon;
		public  string Name { get; private set; }

		public void Construct()
		{
			if (_isConstructed)
				return;
			
			Name = Enum.GetName(typeof(Upgrade), _upgradeType);
			_isConstructed = true;
		}
	}
}