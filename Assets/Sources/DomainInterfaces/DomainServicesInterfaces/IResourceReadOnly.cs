using System;

namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface IResourceReadOnly<out T>
	{
		event Action<T> ResourceChanged;
		T Count { get; }
	}
}