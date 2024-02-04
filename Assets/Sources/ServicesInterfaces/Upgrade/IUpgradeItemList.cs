namespace Sources.ServicesInterfaces.Upgrade
{
	public interface IUpgradeItemList
	{
		IUpgradeItemData[] Items { get; }
		IUpgradeItemPrefab[] Prefabs { get; }
	}
}