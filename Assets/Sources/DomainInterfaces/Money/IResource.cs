using System;
using Domain.Progress.ResourcesData;

namespace DomainInterfaces.Money
{
	public interface IResource<in T>
	{
		ResourceType ResourceType { get; }
		int Count { get; set; }

		event Action<int> CountChanged;
	}
}