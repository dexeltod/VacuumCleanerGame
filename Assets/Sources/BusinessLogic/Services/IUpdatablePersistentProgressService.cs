using Sources.DomainInterfaces;

namespace Sources.BusinessLogic.Services
{
	public interface IUpdatablePersistentProgressService : IPersistentProgressService
	{
		void Update(IGlobalProgress progress);
	}
}