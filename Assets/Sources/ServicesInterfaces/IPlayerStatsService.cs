using Sources.DIService;
using Sources.DomainInterfaces;

namespace Sources.ServicesInterfaces
{
	public interface IPlayerStatsService : IService
	{
		void Set(string name, int value);
		IPlayerStat GetPlayerStat(string name);
	}
}