using Sources.Domain.Temp;

namespace Sources.InfrastructureInterfaces.Repository
{
	public interface IModifiableStatsRepository
	{
		void Increase(int id, int value);
		void Decrease(int id, int value);
		void Clear(int id);
		IStat Get(int id);
	}
}