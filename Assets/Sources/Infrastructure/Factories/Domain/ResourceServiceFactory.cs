using System.Collections.Generic;
using Sources.Domain.Progress.ResourcesData;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Utils;

namespace Sources.Infrastructure.Factories
{
	public class ResourceServiceFactory
	{
		private readonly Dictionary<ResourceType, IResource<float>> _floatResources = new();
		private readonly Dictionary<ResourceType, IResource<int>> _intResources = new();

		public ResourceServiceFactory()
		{
			IResource<int> intResourceSoft = new IntResource(ResourceType.Soft);
			IResource<int> intResourceHard = new IntResource(ResourceType.Hard);
			IResource<int> intResourceScore = new IntResource(ResourceType.CashScore);
			IResource<int> intResourceGlobalScore = new IntResource(ResourceType.GlobalScore);

			_intResources.Add(ResourceType.Soft, intResourceSoft);
			_intResources.Add(ResourceType.Hard, intResourceHard);
			_intResources.Add(ResourceType.CashScore, intResourceScore);
			_intResources.Add(ResourceType.GlobalScore, intResourceGlobalScore);
		}

		public Dictionary<ResourceType, IResource<int>> GetIntResources() =>
			_intResources;

		public Dictionary<ResourceType, IResource<float>> GetFloatResources() =>
			_floatResources;
	}
}