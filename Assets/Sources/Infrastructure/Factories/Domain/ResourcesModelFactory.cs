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

		private readonly IResourceService _resourceService;

		public ResourcesModelFactory(IResourceService resourceService) =>
			_resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));

		public override ResourceModel Create()
		{
			Resource<int> soft = GetResource(CurrencyResourceType.Soft);
			Resource<int> hard = GetResource(CurrencyResourceType.Hard);
			Resource<int> cashScore = GetResource(CurrencyResourceType.CashScore);
			Resource<int> globalScore = GetResource(CurrencyResourceType.GlobalScore);

			return CreateResourceModel(soft, hard, cashScore, globalScore);
		}

		private ResourceModel CreateResourceModel(
			Resource<int> soft,
			Resource<int> hard,
			Resource<int> cashScore,
			Resource<int> globalScore
		) =>
			new(
				soft,
				hard,
				cashScore,
				globalScore,
				StartScoreCount,
				StartCurrencyCount,
				StartScoreCount
			);

		private Resource<int> GetResource(CurrencyResourceType type) =>
			_resourceService.GetResource<int>(type) as Resource<int>;
	}
}