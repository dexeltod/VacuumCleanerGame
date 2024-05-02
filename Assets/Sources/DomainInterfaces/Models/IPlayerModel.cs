using Sources.DomainInterfaces.Entities;

namespace Sources.DomainInterfaces.Models
{
	public interface IPlayerModel
	{
		void Set(int id, float value);
		IStatReadOnly Get(int id);
	}
}