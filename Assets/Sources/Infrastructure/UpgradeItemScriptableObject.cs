using System;
using System.Collections.Generic;
using Sources.View.UI.Shop;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.View.ScriptableObjects.UpgradeItems.SO
{
	[CreateAssetMenu(fileName = "Item", menuName = "Data/Shop/Upgrade/Item")]
	public class UpgradeItemScriptableObject : ScriptableObject
	{
		private const int MaxPoints = 6;

		public enum Upgrade
		{
			Speed,
			VacuumDistance,
			SandCount,
		}

		[FormerlySerializedAs("_upgradeElement")] [SerializeField]
		private UpgradeElementView _upgradeElementView;

		[SerializeField] private string _title;
		[SerializeField] private string _description;
		[SerializeField] private Sprite _icon;
		[SerializeField] private Upgrade _upgradeType;
		[SerializeField] private List<int> _prices = new(MaxPoints);

		private string _progressName;
		private int _upgradeLevel = 0;

		public UpgradeElementView UpgradeElementView => _upgradeElementView;
		public Upgrade UpgradeType => _upgradeType;
		public string Title => _title;
		public string Description => _description;
		public int Price => _prices[_upgradeLevel];
		public Sprite Icon => _icon;
		public int UpgradeLevel => _upgradeLevel;

		public event Action<int> PriceChanged;

		public string GetProgressName()
		{
			_progressName ??= Enum.GetName(typeof(Upgrade), UpgradeType);
			return _progressName;
		}

		public void SetUpgradeLevel(int level)
		{
			_upgradeLevel = level;
			PriceChanged?.Invoke(_prices[_upgradeLevel]);
		}

		private void OnValidate()
		{
			if (_upgradeLevel <= 0)
				_upgradeLevel = 1;

			if (_prices.Count > MaxPoints)
				for (int i = MaxPoints; i < _prices.Count; i++)
					_prices.RemoveAt(i);

			if (_prices.Count < MaxPoints)
				for (int i = _prices.Count; i < MaxPoints; i++)
					_prices.Add(0);
		}
	}
}