using Sources.DomainInterfaces.Entities;
using Sources.Utils.Enums;

namespace Sources.BusinessLogic.Repository
{
	public interface IPlayerModelRepository
	{
		IStatReadOnly Get(ProgressType type);
		IStatReadOnly Get(int id);
		void Set(ProgressType type, float value);
	}
}