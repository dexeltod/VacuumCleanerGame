using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Domain.Progress;
using Sources.Domain.Progress.Player;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Factories.Player;
using Sources.Infrastructure.Repositories;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Providers;
using Sources.ServicesInterfaces;
using Sources.Utils;
using Sources.Utils.ConstantNames;
using VContainer;

namespace Sources.Infrastructure.Factories.Domain
{
	public class InitialProgressFactory : IInitialProgressFactory
	{
		private readonly IResourceService _resourceService;
		private readonly UpgradeProgressRepositoryProvider _upgradeProgressRepositoryProvider;
		private readonly IAssetFactory _assetFactory;
		// private readonly IModifiableStatsRepositoryProvider _modifiableStatsRepositoryProvider;

		[Inject]
		public InitialProgressFactory(
			IResourceService resourceService,
			ProgressConstantNames progressConstantNames,
			IPlayerProgressSetterFacadeProvider playerProgressSetterFacadeProvider,
			IPersistentProgressServiceProvider persistentProgressServiceProvider,
			UpgradeProgressRepositoryProvider upgradeProgressRepositoryProvider,
			IAssetFactory assetFactory,
			// IModifiableStatsRepositoryProvider modifiableStatsRepositoryProvider
		)
		{
			_resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));
			_upgradeProgressRepositoryProvider = upgradeProgressRepositoryProvider ??
				throw new ArgumentNullException(nameof(upgradeProgressRepositoryProvider));
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_modifiableStatsRepositoryProvider = modifiableStatsRepositoryProvider ??
				throw new ArgumentNullException(nameof(modifiableStatsRepositoryProvider));
		}

		public IGlobalProgress Create() =>
			new GlobalProgress(
				new ResourcesModelFactory(_resourceService).Create(),
				new LevelProgressFactory(
					firstLevel: 1,
					GameConfig.DefaultMaxTotalResource
				).Create(),
				new ShopModelFactory(_assetFactory).LoadList(),
				new PlayerModelFactory(_modifiableStatsRepositoryProvider, _assetFactory).Create()
			);
	}
}