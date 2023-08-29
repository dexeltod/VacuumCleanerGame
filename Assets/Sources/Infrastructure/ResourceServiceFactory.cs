using System.Collections.Generic;
using Sources.Application.Utils;
using Sources.Domain.Progress.ResourcesData;
using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.Infrastructure
{
	public class ResourceServiceFactory
	{
		private readonly Dictionary<ResourceType, IResource<float>> _floatResources = new();
		private readonly Dictionary<ResourceType, IResource<int>> _intResources = new();

		public ResourceServiceFactory()
		{
			IResource<int> intResourceSoft = new IntResource(ResourceType.Soft);
			IResource<int>  intResourceHard = new IntResource(ResourceType.Hard);

			_intResources.Add(ResourceType.Soft, intResourceSoft);
			_intResources.Add(ResourceType.Hard, intResourceHard);
		}

		public Dictionary<ResourceType, IResource<int>> GetIntResources() => 
			_intResources;

		public Dictionary<ResourceType, IResource<float>> GetFloatResources() => 
			_floatResources;
	}
}