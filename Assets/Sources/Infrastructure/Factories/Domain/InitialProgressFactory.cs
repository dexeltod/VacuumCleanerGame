using System;
using System.Collections.Generic;
using Sources.Domain.Progress;
using Sources.Domain.Progress.Player;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.DataViewModel;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Providers;
using Sources.ServicesInterfaces.DTO;
using Sources.ServicesInterfaces.Upgrade;
using Sources.Utils.ConstantNames;
using VContainer;

namespace Sources.Infrastructure.Factories.Domain
{
	public class InitialProgressFactory
	{
		private const int MaxUpgradePointsCount = 6;

		private readonly IProgressUpgradeFactory _progressUpgradeFactory;
		private readonly IResourceService _resourceService;
		private readonly ProgressConstantNames _progressConstantNames;
		private readonly IPlayerProgressSetterFacadeProvider _playerProgressSetterFacadeProvider;
		private readonly IPlayerStatsServiceProvider _playerStatsService;
		private readonly IPersistentProgressServiceProvider _persistentProgressServiceProvider;

		private IPersistentProgressService PersistentProgressService =>
			_persistentProgressServiceProvider.Implementation;

		[Inject]
		public InitialProgressFactory(
			IProgressUpgradeFactory progressUpgradeFactory,
			IResourceService resourceService,
			ProgressConstantNames progressConstantNames,
			IPlayerProgressSetterFacadeProvider playerProgressSetterFacadeProvider,
			IPlayerStatsServiceProvider playerStatsService,
			IPersistentProgressServiceProvider persistentProgressServiceProvider
		)
		{
			_progressUpgradeFactory = progressUpgradeFactory ??
				throw new ArgumentNullException(nameof(progressUpgradeFactory));
			_resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));
			_progressConstantNames
				= progressConstantNames ?? throw new ArgumentNullException(nameof(progressConstantNames));
			_playerProgressSetterFacadeProvider = playerProgressSetterFacadeProvider ??
				throw new ArgumentNullException(nameof(playerProgressSetterFacadeProvider));
			_playerStatsService = playerStatsService ?? throw new ArgumentNullException(nameof(playerStatsService));
			_persistentProgressServiceProvider = persistentProgressServiceProvider ??
				throw new ArgumentNullException(nameof(persistentProgressServiceProvider));
		}

		public IGameProgressProvider Create() =>
			Initialize(_progressUpgradeFactory.LoadItems());

		private GameProgressProvider Initialize(IUpgradeItemData[] itemsList)
		{
			
			_playerProgressSetterFacadeProvider.Register<IPlayerProgressSetterFacade>(
				new PlayerProgressSetterFacade(_playerStatsService, PersistentProgressService)
			);

			ResourcesModel resourcesModel = new ResourcesModelFactory(_resourceService).Create();
			PlayerProgress playerProgressModel = new PlayerProgressFactory(itemsList).Create();
			List<ProgressUpgradeData> progressUpgradeData = new ProgressUpgradeDataFactory(itemsList).Create();
			LevelProgress levelProgressModel = new LevelProgressFactory(_progressConstantNames).Create();
			UpgradeProgressModel upgradeProgressModelModel = new(progressUpgradeData, MaxUpgradePointsCount);

			return new GameProgressProvider(
				resourcesModel,
				playerProgressModel,
				upgradeProgressModelModel,
				levelProgressModel
			);
		}
	}
}