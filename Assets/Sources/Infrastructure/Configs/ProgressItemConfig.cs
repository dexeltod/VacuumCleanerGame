using System;
using System.Collections.Generic;
using System.Linq;
using Sources.BusinessLogic.Interfaces.Configs;
using Sources.Utils;
using Sources.Utils.Enums;
using UnityEngine;

namespace Sources.Infrastructure.Configs
{
	[Serializable]
	public class ProgressItemConfig
	{
		[SerializeField] private List<ShopProgressConfig> _items;
		[SerializeField] private string _title;
		[SerializeField] private string _description;

		[SerializeField] private ProgressType _type;
		[SerializeField] private bool _isModifiable;

		[SerializeField] private AnimationCurve _priceCurve;

		public IEnumerable<IShopProgress> Items => _items;

		public ProgressType Type => _type;
		public int MaxProgressCount => _items.Count;
		public int MaxProgress => (int)_items.Last().Progress;
		public int StartProgress => (int)_items[0].Progress;
		public int Id => StaticIdRepository.GetOrAddByEnum(Type);
		public bool IsModifiable => _isModifiable;
		public string Description => _description;

		public string Title => _title;
	}
}
