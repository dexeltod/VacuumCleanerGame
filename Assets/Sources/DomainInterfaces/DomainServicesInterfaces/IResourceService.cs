using Sources.Application.Utils;
using Sources.DIService;

namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface IResourceService : IService
	{
		IResource<T> GetResource<T>(ResourceType type);

		void Increase<T>(ResourceType type, T value);
		void Decrease<T>(ResourceType type, T value);
		void Set<T>(ResourceType type, T value);
	}
}