using Sources.Utils;

namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface IResourceService
	{
		IResource<T> GetResource<T>(CurrencyResourceType type);

		void Set<T>(CurrencyResourceType type, T value);
	}
}