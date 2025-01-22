using System;
using Sources.Domain.Progress.Player;
using Sources.Domain.Progress.ResourcesData;
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
				GetResource((int)CurrencyResourceType.Soft) as IntCurrency,
				GetResource((int)CurrencyResourceType.Hard) as IntCurrency,
				GetResource((int)CurrencyResourceType.CashScore) as IntCurrency,
				GetResource((int)CurrencyResourceType.GlobalScore) as IntCurrency,
				StartScoreCount,
				StartCurrencyCount,
				StartScoreCount
			);

		private Resource<int> GetResource(int type) =>
			_resourcesRepository.GetResource<int>(type) as Resource<int>;
	}
}