using System;
using System.Collections.Generic;
using Sources.ServicesInterfaces.Upgrade;
using Sources.Utils.Enums;
using UnityEngine;

namespace Sources.Infrastructure.Configs
{
	[Serializable] public class ProgressItemConfig : IProgressItemConfig
	{
		[SerializeField] private List<int> _prices;
		[SerializeField] private List<float> _stats;

		[SerializeField] private string _title;
		[SerializeField] private string _description;

		[SerializeField] private ProgressType _type;
		[SerializeField] private bool _isModifiable = false;

		public ProgressType Type => _type;
		public int Id => (int)_type;
		public int MaxProgressCount => _prices.Count;
		public bool IsModifiable => _isModifiable;
		public string Description => _description;
		public IReadOnlyList<int> Prices => _prices;

		public IReadOnlyList<float> Stats => _stats;

		public string Title => _title;
	}
}