using System;
using System.Collections.Generic;
using Sources.InfrastructureInterfaces;
using UnityEngine;

namespace Sources.Infrastructure.ScriptableObjects.Shop
{
	public class ProgressItem : ScriptableObject, IUpgradeItem
	{
		private const int MaxPoints = 6;

		[SerializeField] private List<int> _prices = new(MaxPoints);
		[SerializeField] private string _title;
		[SerializeField] private string _description;

		public int Price { get; }
		public int PointLevel { get; }
		public event Action<int> PriceChanged;

		public string Name { get; }

		public string Title => _title;

		public string Description => _description;

		private string _progressName;
		private int _pointLevel;

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