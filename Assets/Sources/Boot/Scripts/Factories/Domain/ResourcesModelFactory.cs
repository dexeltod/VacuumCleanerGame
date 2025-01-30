using System;
using Sources.Domain.Progress.Entities.Values;
using Sources.Domain.Progress.Player;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Utils;

namespace Sources.Boot.Scripts.Factories.Domain
{
	public class ResourcesModelFactory
	{
		private readonly IResourcesRepository _resourcesRepository;

		public ResourcesModelFactory(IResourcesRepository resourcesRepository) =>
			_resourcesRepository = resourcesRepository ?? throw new ArgumentNullException(nameof(resourcesRepository));

		public ResourceModel Create()
		{
			var resource = new ResourceModel(
				GetResource(StaticIdRepository.GetByEnum(ResourceType.Soft)),
				GetResource(StaticIdRepository.GetByEnum(ResourceType.Hard)),
				GetResource(StaticIdRepository.GetByEnum(ResourceType.CashScore)),
				GetResource(StaticIdRepository.GetByEnum(ResourceType.GlobalScore))
			);
			return resource;
		}

		private IntEntity GetResource(int type) => _resourcesRepository.GetResource<int>(type) as IntEntity;
	}
}
