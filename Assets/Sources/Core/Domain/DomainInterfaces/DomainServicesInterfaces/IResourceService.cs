using System;
using Sources.Core.Domain.Progress.ResourcesData;

namespace Sources.Core.Domain.DomainInterfaces.DomainServicesInterfaces
{
	public interface IResourceService<in T>
	{
		event Action<IResource<T>> ResourceChanged;
		void Increase(ResourceType type, int value);
		void Decrease(ResourceType type, int value);
	}
}