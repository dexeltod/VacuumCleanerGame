using System;

namespace Sources.InfrastructureInterfaces.Upgrade
{
	public interface IUpgradeItemData
	{
		int Price { get; }
		int PointLevel { get; }
		event Action<int> PriceChanged;
		void SetUpgradeLevel(int level);
		string IdName { get; }
		string Title { get; }
		string Description { get; }
		int MaxPointLevel { get; }
		int[] Stats { get; }
	}
}