using System;
using Sources.DomainInterfaces;
using UnityEngine;

namespace Sources.Domain.Progress
{
	[Serializable] public class ProgressUpgradeData : IUpgradeProgressData
	{
		[SerializeField] private int _value;
		[SerializeField] private string _name;
		[SerializeField] private int _itemDataMaxPointLevel;

		public ProgressUpgradeData(string name, int pointCount, int itemDataMaxPointLevel)
		{
			_itemDataMaxPointLevel = itemDataMaxPointLevel;
			_name = name;
			_value = pointCount;
		}

		public string Name => _name;
		public int MaxPointLevel => _itemDataMaxPointLevel;

		public int Value
		{
			get => _value;

			set
			{
				if (value < 0)
					throw new Exception("Value is negative");

				_value = value;
			}
		}
	}
}