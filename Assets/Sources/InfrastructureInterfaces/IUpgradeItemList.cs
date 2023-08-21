namespace Sources.InfrastructureInterfaces
{
	public interface IUpgradeItemList
	{
		IUpgradeItemData[] Items { get; }
		IUpgradeItemPrefab[] Prefabs { get; }
	}
}