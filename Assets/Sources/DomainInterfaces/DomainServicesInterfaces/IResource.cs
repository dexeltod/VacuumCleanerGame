using System;
using Sources.Application.Utils;

namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface IResource<in T>
	{
		ResourceType ResourceType { get; }
		int Count { get; set; }

		event Action<int> CountChanged;
	}
}