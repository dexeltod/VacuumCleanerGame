using Sources.ServicesInterfaces.Upgrade;

namespace Sources.InfrastructureInterfaces.Configs
{
	public interface IUpgradeItemListConfig
	{
		IUpgradeItemData[] Items { get; }
		// UpgradeItemConfig[] Configs { get; }
	}
}