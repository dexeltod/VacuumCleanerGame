using Sources.DomainInterfaces;

namespace Sources.BusinessLogic.Services
{
	public interface IUpdatablePersistentProgressService
	{
		void Update(IGlobalProgress progress);
	}
}
