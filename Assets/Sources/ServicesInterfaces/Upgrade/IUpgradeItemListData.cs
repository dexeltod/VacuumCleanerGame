namespace Sources.ServicesInterfaces.Upgrade
{
	public interface IUpgradeItemListData
	{
		IUpgradeItemData[] Items { get; }
		IUpgradeItemPrefab[] Prefabs { get; }
	}
}