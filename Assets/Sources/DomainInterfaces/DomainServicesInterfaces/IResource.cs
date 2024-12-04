namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface IResource<T> : IReadOnlyProgressValue<T>
	{
		void Set(T value);
	}
}