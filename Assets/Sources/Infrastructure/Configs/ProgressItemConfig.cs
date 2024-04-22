using System;
using System.Collections.Generic;
using Sources.ServicesInterfaces.Upgrade;
using Sources.Utils;
using UnityEngine;

namespace Sources.Infrastructure.ScriptableObjects.Shop
{
	[Serializable] public class ProgressItemConfig : ScriptableObject, IProgressItemConfig
	{
		[SerializeField] private List<int> _prices;

		[SerializeField] private string _title;
		[SerializeField] private string _description;
		[SerializeField] private List<int> _stats;

		[SerializeField] private ProgressType _type;
		[SerializeField] private bool _isModifiable = false;

		public ProgressType Type => _type;
		public int Id => (int)_type;
		public int MaxProgressCount => _prices.Count;
		public bool IsModifiable => _isModifiable;
		public string Description => _description;
		public IReadOnlyList<int> Prices => _prices;

		public IReadOnlyList<int> Stats => _stats;

		public string Title => _title;

		private void OnValidate()
		{
			if (_prices.Count > _stats.Count)
				for (int i = _stats.Count; i < _prices.Count; i++)
					_prices.RemoveAt(i);

			if (_prices.Count >= _stats.Count) return;

			for (int i = _prices.Count; i < _stats.Count; i++)
				_prices.Add(0);
		}
	}
}