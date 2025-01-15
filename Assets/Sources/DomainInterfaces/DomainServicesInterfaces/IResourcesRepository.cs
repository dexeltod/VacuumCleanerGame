namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface IResourcesRepository
	{
		IResource<T> GetResource<T>(int id);

		void Set<T>(int id, T value);
	}
}
