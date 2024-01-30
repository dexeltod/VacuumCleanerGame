using System;
using System.Collections.Generic;
using Sources.InfrastructureInterfaces.Upgrade;
using UnityEngine;

namespace Sources.Infrastructure.ScriptableObjects.Shop
{
	[Serializable] public class ProgressItemData : ScriptableObject, IUpgradeItemData, IItemChangeable
	{
		private const int MaxPoint = 6;

		[SerializeField] private List<int> _prices = new(MaxPoint);

		[SerializeField] private string _title;
		[SerializeField] private string _description;
		[SerializeField] private int[] _stats;

		private int _pointIndex;

		public string Description => _description;
		public int MaxPointLevel => MaxPoint;
		public int PointLevel => _pointIndex;

		public int Price
		{
			get
			{
				if (_pointIndex < _prices.Count - 1)
					return _prices[_pointIndex];

				_pointIndex = _prices.Count - 1;
				return _prices[_pointIndex];
			}
		}

		public int[] Stats => _stats;
		public string IdName => name;
		public string Title => _title;

		public event Action<int> PriceChanged;

		public void SetUpgradeLevel(int level)
		{
			_pointIndex = level;

			PriceChanged?.Invoke(Price);
		}

		private void OnValidate()
		{
			if (_pointIndex <= 0)
				_pointIndex = 0;

			if (_prices.Count > MaxPoint)
				for (int i = MaxPoint; i < _prices.Count; i++)
					_prices.RemoveAt(i);

			if (_prices.Count < MaxPoint)
			{
				for (int i = _prices.Count; i < MaxPoint; i++)
					_prices.Add(0);
			}
		}
	}
}