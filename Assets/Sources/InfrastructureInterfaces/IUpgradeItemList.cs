using Sources.PresentationInterfaces;

namespace Sources.InfrastructureInterfaces
{
	public interface IUpgradeItemList
	{
		IUpgradeItemPrefabData[] Items { get; }
	}
}