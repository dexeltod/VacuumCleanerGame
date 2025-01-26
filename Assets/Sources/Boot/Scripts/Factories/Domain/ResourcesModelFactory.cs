using System;
using Sources.Domain.Progress.Entities.Values;
using Sources.Domain.Progress.Player;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Utils;

namespace Sources.Boot.Scripts.Factories.Domain
{
	public class ResourcesModelFactory
	{
		private const int StartCurrencyCount = 0;
		private const int StartScoreCount = 0;

		private readonly IResourcesRepository _resourcesRepository;

		public ResourcesModelFactory(IResourcesRepository resourcesRepository) =>
			_resourcesRepository = resourcesRepository ?? throw new ArgumentNullException(nameof(resourcesRepository));

		public ResourceModel Create() =>
			new(
				GetResource((int)CurrencyResourceType.Soft),
				GetResource((int)CurrencyResourceType.Hard),
				GetResource((int)CurrencyResourceType.CashScore),
				GetResource((int)CurrencyResourceType.GlobalScore),
				StartScoreCount,
				StartCurrencyCount,
				StartScoreCount
			);

		private IntEntityValue GetResource(int type) => _resourcesRepository.GetResource<int>(type) as IntEntityValue;
	}
}