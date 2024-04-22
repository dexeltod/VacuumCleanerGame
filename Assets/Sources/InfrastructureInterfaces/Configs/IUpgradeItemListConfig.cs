using Sources.ServicesInterfaces.Upgrade;

namespace Sources.InfrastructureInterfaces.Configs
{
	public interface IUpgradeItemListConfig
	{
		IProgressItemConfig[] Items { get; }
		// UpgradeItemConfig[] Configs { get; }
	}
}