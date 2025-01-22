using Sources.DomainInterfaces.Entities;

namespace Sources.DomainInterfaces.Models
{
	public interface IPlayerStatsModel
	{
		IStatReadOnly Get(int id);
		void Set(int id, float value);
	}
}