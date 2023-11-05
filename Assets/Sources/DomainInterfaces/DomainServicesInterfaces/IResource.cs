using Sources.Utils;

namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface IResource<T> : IResourceReadOnly<T>
	{
		ResourceType ResourceType { get; }

		void Set(T value);
	}
}