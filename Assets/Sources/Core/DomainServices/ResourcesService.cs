using System;
using System.Collections.Generic;
using Sources.Core.Domain.DomainInterfaces.DomainServicesInterfaces;
using Sources.Core.Domain.Progress.ResourcesData;

namespace Sources.DomainServices
{
	public class ResourcesService : IResourceService<int>
	{
		private readonly Dictionary<ResourceType, Resource> _resources;
		public event Action<IResource<int>> ResourceChanged;

		public ResourcesService(Dictionary<ResourceType, Resource> resources)
		{
			_resources = resources;
		}

		public void Increase(ResourceType type, int value)
		{
			SeeContaining(type);
		}

		public void Decrease(ResourceType type, int value)
		{
			SeeContaining(type);
		}

		private void SeeContaining(ResourceType type)
		{
			if (_resources.ContainsKey(type) == false)
				throw new ArgumentException("Resource does not exists");
		}
	}
}