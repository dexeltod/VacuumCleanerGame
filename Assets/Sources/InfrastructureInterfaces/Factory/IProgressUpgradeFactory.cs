
using Sources.ServicesInterfaces.Upgrade;

namespace Sources.InfrastructureInterfaces.Factory
{
	public interface IProgressUpgradeFactory 
	{
		IUpgradeItemData[] LoadItems();
	}
}