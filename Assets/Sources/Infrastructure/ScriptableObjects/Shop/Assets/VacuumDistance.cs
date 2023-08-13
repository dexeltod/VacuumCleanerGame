using System;
using Sources.DomainInterfaces;

namespace Sources.Infrastructure.ScriptableObjects.Shop.Assets
{
	public class VacuumDistance : IUpgradeProgressData, IShopData
	{
		public VacuumDistance(int progressPoint)
		{
			ProgressPoint = progressPoint;
		}

		public string Name => "VacuumDistance";
		public int Value { get; set; }
		public event Action Changed;
		public int ProgressPoint { get; }
	}
}