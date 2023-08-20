using System;
using System.Collections.Generic;
using Sources.InfrastructureInterfaces;
using UnityEngine;

namespace Sources.Infrastructure.ScriptableObjects.Shop
{
	public class ProgressItemData : ScriptableObject, IUpgradeItemData
	{
		private const int MaxPoints = 6;

		[SerializeField] private List<int> _prices = new(MaxPoints);
		[SerializeField] private string _title;
		[SerializeField] private string _description;

		private string _progressName;
		private int _pointLevel;

		public int Price => _prices[_pointLevel];
		public int PointLevel => _pointLevel;
		public string IdName => name;
		public string Title => _title;
		public string Description => _description;

		public event Action<int> PriceChanged;

		public void SetUpgradeLevel(int level)
		{
			_pointLevel = level;
			PriceChanged?.Invoke(_prices[_pointLevel]);
		}

		private void OnValidate()
		{
			if (_pointLevel <= 0)
				_pointLevel = 1;

			if (_prices.Count > MaxPoints)
				for (int i = MaxPoints; i < _prices.Count; i++)
					_prices.RemoveAt(i);

			if (_prices.Count < MaxPoints)
				for (int i = _prices.Count; i < MaxPoints; i++)
					_prices.Add(0);
		}
	}
}