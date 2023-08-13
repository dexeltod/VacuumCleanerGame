using System;
using Sources.DomainInterfaces;

namespace Sources.Infrastructure.ScriptableObjects.Shop.Assets
{
	public class Speed : IUpgradeProgressData, IShopData
	{
		private int _speed;
		public int ProgressPoint { get; }
		public string Name { get; }

		public int Value
		{
			get => _speed;
			set
			{
				if (value < 0)
					throw new Exception("Value is negative");

				_speed = value;
			}
		}

		public event Action Changed;

		public Speed(int progressPoint, string name)
		{
			Name = name;
			ProgressPoint = progressPoint;
		}
	}

	public interface IShopData
	{
		public int ProgressPoint { get; }
	}
}