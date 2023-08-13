using System;
using Sources.Application.Utils;

namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface IResourceService<in T>
	{
		event Action<IResource<T>> ResourceChanged;
		void Increase(ResourceType type, int value);
		void Decrease(ResourceType type, int value);
	}
}