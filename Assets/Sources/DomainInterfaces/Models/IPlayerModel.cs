using Sources.Domain.Temp;

namespace Sources.DomainInterfaces.Models
{
	public interface IPlayerModel
	{
		void Set(int id, float value);
		IStatReadOnly Get(int id);
	}
}