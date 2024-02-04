namespace Sources.ServicesInterfaces.Upgrade
{
	public interface IUpgradeElementConstructable
	{
		IUpgradeElementConstructable Construct(
			IItemChangeable itemChangeable ,
			IUpgradeItemData itemData,
			IUpgradeItemPrefab viewInfo,
			string title,
			string description
		);
	}
}