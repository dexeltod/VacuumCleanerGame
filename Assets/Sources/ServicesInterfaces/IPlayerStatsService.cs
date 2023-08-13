using Sources.DIService;

namespace Sources.ServicesInterfaces
{
	public interface IPlayerStatsService : IService
	{
		void Set(string name, int value);
		int GetConvertedProgressValue(string name);
	}
}