using System;
using Sources.Domain.Progress;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Providers;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Providers;
using Sources.ServicesInterfaces;
using Sources.Utils.AssetPaths;
using Sources.Utils.ConstantNames;
using VContainer;

namespace Sources.Infrastructure.Factories.Domain
{
	public class InitialProgressFactory : IInitialProgressFactory
	{
		private readonly IResourceService _resourceService;
		private readonly IAssetFactory _assetFactory;

		[Inject]
		public InitialProgressFactory(
			IResourceService resourceService,
			ProgressConstantNames progressConstantNames,
			IPersistentProgressServiceProvider persistentProgressServiceProvider,
			ProgressEntityRepositoryProvider progressEntityRepositoryProvider,
			IAssetFactory assetFactory
		)
		{
			_resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));
			_assetFactory
				= assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
		}

		public IGlobalProgress Create()
		{
			ShopModel shopModelFactory = new ShopModelFactory(_assetFactory).LoadList();

			return new GlobalProgress(
				new ResourcesModelFactory(_resourceService).Create(),
				new LevelProgressFactory(
					firstLevel: 1,
					GameConfig.DefaultMaxTotalResource
				).Create(),
				shopModelFactory,
				new PlayerModelFactory(_assetFactory, shopModelFactory).Create()
			);
		}
	}
}