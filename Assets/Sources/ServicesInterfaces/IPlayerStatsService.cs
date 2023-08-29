using Sources.DIService;

namespace Sources.ServicesInterfaces
{
	public interface IPlayerStatsService : IService
	{
		void Set(string name, int value);
		IPlayerStat GetPlayerStat(string name);
	}
}