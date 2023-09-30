using Sources.Utils;

namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface IResource<T> : IResourceChangeable<T>
	{
		ResourceType ResourceType { get; }
		T            Count        { get; }

		void Set(T value);
	}
}