using Sources.PresentationInterfaces;

namespace Sources.InfrastructureInterfaces
{
	public interface IUpgradeItemList
	{
		IUpgradeItemData[] Items { get; }
		IUpgradeItemPrefabData[] Prefabs { get; }
	}
}