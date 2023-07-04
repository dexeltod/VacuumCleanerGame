using System;
using System.Collections.Generic;
using Domain.Progress.ResourcesData;
using DomainInterfaces.Money;

namespace DomainServices
{
	public class ResourcesService : IResourceService<int>
	{
		private readonly Dictionary<ResourceType, Resource> _resources;

		public event Action<IResource<int>> ResourceChanged;

		public void Increase(ResourceType type, int value)
		{
			if (_resources.ContainsKey(type) == false)
				throw new ArgumentException("Resource does not exists");
		}

		public void Decrease(ResourceType type, int value)
		{
			throw new System.NotImplementedException();
		}
	}

	public interface IResourceService<in T>
	{
		event Action<IResource<T>> ResourceChanged;
		void Increase(ResourceType type, int value);
		void Decrease(ResourceType type, int value);
	}
}