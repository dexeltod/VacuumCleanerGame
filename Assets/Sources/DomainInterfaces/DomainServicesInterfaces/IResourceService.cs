using Sources.DIService;
using Sources.Utils;

namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface IResourceService : IService
	{
		IResource<T> GetResource<T>(ResourceType type);

		void Set<T>(ResourceType type, T value);
	}
}