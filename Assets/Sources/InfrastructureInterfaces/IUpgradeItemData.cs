using System;

namespace Sources.InfrastructureInterfaces
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
	}
}