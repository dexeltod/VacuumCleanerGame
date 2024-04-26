using Sources.Domain.Temp;
using Sources.Utils;

namespace Sources.InfrastructureInterfaces.Repository
{
	public interface IPlayerModelRepository
	{
		IStatReadOnly Get(ProgressType type);
		IStatReadOnly Get(int id);
		void Set(ProgressType type, int value);
	}
}