using System;
using Sources.Domain.Progress.Player;
using Sources.Domain.Progress.ResourcesData;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Common.Factory;
using Sources.Utils;

namespace Sources.Infrastructure.Factories.Domain
{
	public class ResourcesModelFactory : Factory<ResourceModel>
	{
		private const int StartCurrencyCount = 0;
		private const int StartScoreCount = 0;

		private readonly IResourcesRepository _resourcesRepository;

		public ResourcesModelFactory(IResourcesRepository resourcesRepository) =>
			_resourcesRepository = resourcesRepository ?? throw new ArgumentNullException(nameof(resourcesRepository));

		public override ResourceModel Create()
		{
			Resource<int> soft = GetResource(CurrencyResourceType.Soft);
			Resource<int> hard = GetResource((int)CurrencyResourceType.Hard);
			Resource<int> cashScore = GetResource((int)CurrencyResourceType.CashScore);
			Resource<int> globalScore = GetResource((int)CurrencyResourceType.GlobalScore);

			return new ResourceModel(
				soft,
				hard,
				cashScore,
				globalScore,
				StartScoreCount,
				StartCurrencyCount,
				StartScoreCount
			);
		}

		private Resource<int> GetResource(int type) =>
			_resourcesRepository.GetResource<int>(type) as Resource<int>;
	}
}
