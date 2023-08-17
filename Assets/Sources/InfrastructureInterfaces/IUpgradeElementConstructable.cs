using Sources.InfrastructureInterfaces;

namespace Sources.PresentationInterfaces
{
	public interface IUpgradeElementConstructable
	{
		IUpgradeElementConstructable Construct(IUpgradeItemData itemData, IUpgradeItemPrefabData prefabDataInfo);
	}
}