using System;
using System.Collections.Generic;
using Sources.InfrastructureInterfaces.Upgrade;
using UnityEngine;

namespace Sources.Infrastructure.ScriptableObjects.Shop
{
	public class ProgressItemData : ScriptableObject, IUpgradeItemData
	{
		private const int MaxPoint = 6;

		[SerializeField] private List<int> _prices = new(MaxPoint);
		[SerializeField] private string _title;
		[SerializeField] private string _description;
		[SerializeField] private int[] _stats;

		private string _progressName;
		private int _pointLevel;

		public int[] Stats => _stats;
		public int Price => _prices[_pointLevel];
		public int PointLevel => _pointLevel;
		public string IdName => name;
		public string Title => _title;
		public string Description => _description;
		public int MaxPointLevel => MaxPoint;

		public event Action<int> PriceChanged;

		public void SetUpgradeLevel(int level)
		{
			_pointLevel = level;
			PriceChanged?.Invoke(_prices[_pointLevel]);
		}

		private void OnValidate()
		{
			if (_pointLevel <= 0)
				_pointLevel = 0;

			if (_prices.Count > MaxPointLevel)
				for (int i = MaxPointLevel; i < _prices.Count; i++)
					_prices.RemoveAt(i);

			if (_prices.Count < MaxPointLevel)
				for (int i = _prices.Count; i < MaxPointLevel; i++)
					_prices.Add(0);
		}
	}
}