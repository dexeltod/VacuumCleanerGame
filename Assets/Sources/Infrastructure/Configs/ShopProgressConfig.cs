using System;
using Sources.BusinessLogic.Interfaces.Configs;
using UnityEngine;

namespace Sources.Infrastructure.Configs
{
	[Serializable]
	public class ShopProgressConfig : IShopProgress
	{
		[SerializeField] private int _price;
		[SerializeField] private float _progress;

		public float Progress => _progress;

		public int Price => _price;
	}
}
