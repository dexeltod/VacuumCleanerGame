
using Sources.Utils;

namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface IResourceService 
	{
		IResource<T> GetResource<T>(ResourceType type);

		void Set<T>(ResourceType type, T value);
	}
}