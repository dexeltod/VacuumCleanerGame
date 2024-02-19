using System;
using Sources.Domain.Progress.Player;
using Sources.Domain.Progress.ResourcesData;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Common.Factory;
using Sources.Utils;

namespace Sources.Infrastructure.Factories.Domain
{
	public class ResourcesModelFactory : Factory<ResourcesModel>
	{
		private readonly IResourceService _resourceService;
		private const int StartCurrencyCount = 99999;

		private const int StartScoreCount = 0;

		public ResourcesModelFactory(IResourceService resourceService) =>
			_resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));

		public override ResourcesModel Create()
		{
			Resource<int> soft = GetResource(ResourceType.Soft);
			Resource<int> hard = GetResource(ResourceType.Hard);
			Resource<int> cashScore = GetResource(ResourceType.CashScore);
			Resource<int> globalScore = GetResource(ResourceType.GlobalScore);

			return CreateResourceModel(soft, hard, cashScore, globalScore);
		}

		private ResourcesModel CreateResourceModel(
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

		private Resource<int> GetResource(ResourceType type) =>
			_resourceService.GetResource<int>(type) as Resource<int>;
	}
}