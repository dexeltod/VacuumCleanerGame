using System;

namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface IResourceChangeable<out T>
	{
		event Action<T> ResourceChanged;
	}
}