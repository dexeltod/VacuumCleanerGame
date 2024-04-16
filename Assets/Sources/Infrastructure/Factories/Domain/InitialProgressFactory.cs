using System;
using Sources.Domain.Progress;
using Sources.Domain.Progress.Player;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Factories.UpgradeShop;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Providers;
using Sources.Utils;
using Sources.Utils.ConstantNames;
using VContainer;

namespace Sources.Infrastructure.Factories.Domain
{
	public class InitialProgressFactory : IInitialProgressFactory
	{
		private readonly ProgressEntityFactory _progressEntityFactory;
		private readonly IResourceService _resourceService;
		private readonly IPlayerStatsServiceProvider _playerStatsService;

		[Inject]
		public InitialProgressFactory(
			ProgressEntityFactory progressEntityFactory,
			IResourceService resourceService,
			ProgressConstantNames progressConstantNames,
			IPlayerProgressSetterFacadeProvider playerProgressSetterFacadeProvider,
			IPersistentProgressServiceProvider persistentProgressServiceProvider
		)
		{
			_progressEntityFactory = progressEntityFactory ??
				throw new ArgumentNullException(nameof(progressEntityFactory));
			_resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));
		}

		public IGlobalProgress Create()
		{
			var itemsList = _progressEntityFactory.Load();

			if (itemsList == null) throw new ArgumentNullException(nameof(itemsList));

			ResourcesModel resourcesModel = new ResourcesModelFactory(_resourceService).Create();

			PlayerProgress playerProgressModel = new PlayerProgressFactory(
				new ProgressUpgradeDataFactory(itemsList).Create()
			).Create();

			UpgradeProgressModel upgradeProgressModelModel = new(
				new ProgressUpgradeDataFactory(itemsList).Create()
			);

			LevelProgress levelProgressModel = new LevelProgressFactory(
				firstLevel: 1,
				GameConfig.DefaultMaxTotalResource
			).Create();

			return new GlobalProgress(
				resourcesModel,
				playerProgressModel,
				upgradeProgressModelModel,
				levelProgressModel
			);
		}
	}
}