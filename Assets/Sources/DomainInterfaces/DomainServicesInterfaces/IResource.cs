namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface IResource<in T>
	{
		void Set(T value);
	}
}
