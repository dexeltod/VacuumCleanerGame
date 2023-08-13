using System;

namespace Sources.InfrastructureInterfaces
{
	public interface IUpgradeItem
	{
		
		int Price { get; }
		int PointLevel { get; }
		event Action<int> PriceChanged;
		void SetUpgradeLevel(int level);
		string Name { get; }
	
		string Title { get; }
		string Description { get; }
	}
}