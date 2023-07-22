using System;
using Sources.Core.Domain.Progress.ResourcesData;

namespace Sources.Core.Domain.DomainInterfaces.DomainServicesInterfaces
{
	public interface IResource<in T>
	{
		ResourceType ResourceType { get; }
		int Count { get; set; }

		event Action<int> CountChanged;
	}
}