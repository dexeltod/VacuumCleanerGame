namespace Sources.InfrastructureInterfaces
{
	public interface IUpgradeElementConstructable
	{
		IUpgradeElementConstructable Construct(IUpgradeItemData itemData, IUpgradeItemPrefabData prefabDataInfo);
	}
}