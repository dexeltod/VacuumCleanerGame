namespace Sources.InfrastructureInterfaces.Upgrade
{
	public interface IUpgradeItemList
	{
		IUpgradeItemData[] Items { get; }
		IUpgradeItemPrefab[] Prefabs { get; }
	}
}