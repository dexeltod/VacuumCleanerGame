using System;
using System.Collections.Generic;
using Sources.Application.Utils;
using Sources.Domain.Progress.ResourcesData;
using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.Services.DomainServices
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