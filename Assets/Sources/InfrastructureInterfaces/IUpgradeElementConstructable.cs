namespace Sources.InfrastructureInterfaces
{
	public interface IUpgradeElementConstructable
	{
		IUpgradeElementConstructable Construct(IUpgradeItemData itemData, IUpgradeItemPrefab prefabInfo);
	}
}