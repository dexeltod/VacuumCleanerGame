using Sources.ServicesInterfaces.Upgrade;

namespace Sources.ServicesInterfaces
{
	public interface IProgressSetterFacade
	{
		bool TryAddOneProgressPoint(string progressName, IUpgradeItemData itemData);
	}
}