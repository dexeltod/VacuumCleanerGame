using Sources.InfrastructureInterfaces.Upgrade;

namespace Sources.InfrastructureInterfaces.Factory
{
	public interface IShopItemFactory
	{
		IUpgradeItemData[] LoadItems();
	}
}