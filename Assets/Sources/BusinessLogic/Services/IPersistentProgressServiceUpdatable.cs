using Sources.DomainInterfaces;

namespace Sources.BusinessLogic.Services
{
	public interface IPersistentProgressServiceUpdatable
	{
		void Update(IGlobalProgress progress);
	}
}
