
using Sources.DomainInterfaces;

namespace Sources.ServicesInterfaces
{
	public interface IPlayerStatsService 
	{
		void Set(string name, int value);
		IPlayerStat Get(string name);
	}
}