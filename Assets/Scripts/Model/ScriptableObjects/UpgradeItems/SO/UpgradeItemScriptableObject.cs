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

		private string _progressName;

		public UpgradeElementView UpgradeElementView => _upgradeElementView;
		public Upgrade UpgradeType => _upgradeType;
		public string Title => _title;
		public string Description => _description;
		public int Price => _price;
		public Sprite Icon => _icon;

		public string GetProgressName()
		{
			_progressName ??= Enum.GetName(typeof(Upgrade), UpgradeType);
			return _progressName;
		}
	}
}