
using Sources.InfrastructureInterfaces.Upgrade;

namespace Sources.InfrastructureInterfaces.Factory
{
	public interface IProgressUpgradeFactory 
	{
		IUpgradeItemData[] LoadItems();
	}
}