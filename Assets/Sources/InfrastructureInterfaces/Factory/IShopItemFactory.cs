using Sources.DIService;
using Sources.InfrastructureInterfaces.Upgrade;

namespace Sources.InfrastructureInterfaces.Factory
{
	public interface IShopItemFactory : IService
	{
		IUpgradeItemData[] LoadItems();
	}
}