using Sources.DomainInterfaces;

namespace Sources.BuisenessLogic.Services
{
	public interface IPersistentProgressServiceUpdatable
	{
		void Update(IGlobalProgress progress);
	}
}