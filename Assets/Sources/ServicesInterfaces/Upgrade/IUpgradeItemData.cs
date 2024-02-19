namespace Sources.ServicesInterfaces.Upgrade
{
	public interface IUpgradeItemData
	{
		int Price { get; }
		int BoughtPointsCount { get; }

		void SetUpgradeLevel(int level);
		string IdName { get; }
		string Title { get; }
		string Description { get; }
		int MaxPointLevel { get; }
		int[] Stats { get; }
	}
}