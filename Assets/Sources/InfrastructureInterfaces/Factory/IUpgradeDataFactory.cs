
using Sources.InfrastructureInterfaces.Upgrade;

namespace Sources.InfrastructureInterfaces.Factory
{
	public interface IUpgradeDataFactory 
	{
		IUpgradeItemData[] LoadItems();
	}
}