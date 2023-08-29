namespace Sources.InfrastructureInterfaces.Upgrade
{
	public interface IUpgradeElementConstructable
	{
		IUpgradeElementConstructable Construct(IUpgradeItemData itemData, IUpgradeItemPrefab prefabInfo);
	}
}