using Sources.DomainInterfaces.Entities;

namespace Sources.DomainInterfaces.Models
{
	public interface IPlayerStatsModel
	{
		void Set(int id, float value);
		IStatReadOnly Get(int id);
	}
}
