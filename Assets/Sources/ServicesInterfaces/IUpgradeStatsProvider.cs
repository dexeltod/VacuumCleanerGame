using Sources.DIService;
using Sources.Domain;

namespace Sources.ServicesInterfaces
{
	public interface IUpgradeStatsProvider : IService
	{
		StatsConfig LoadConfig();
	}
}