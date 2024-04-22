using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sources.Domain.Common;
using Sources.Domain.Temp;
using Sources.DomainInterfaces;
using Sources.Infrastructure.DataViewModel;
using Sources.Infrastructure.Factories.Domain;
using Sources.Infrastructure.Factories.UpgradeShop;
using Sources.Infrastructure.Providers;
using Sources.Infrastructure.Repositories;
using Sources.Infrastructure.Repository;
using Sources.InfrastructureInterfaces.Configs;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Providers;
using Sources.InfrastructureInterfaces.Services;
using Sources.Services.DomainServices;
using Sources.ServicesInterfaces;
using Sources.Utils;
using Sources.Utils.ConstantNames;
using UnityEngine;
using VContainer;

namespace Sources.Infrastructure.Factories
{
	[Serializable] public class ProgressFactory : IProgressFactory
	{
		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
		private readonly IPersistentProgressServiceProvider _persistentProgressServiceProvider;
		private readonly IProgressCleaner _progressCleaner;
		private readonly IResourcesProgressPresenterProvider _resourcesProgressPresenterProvider;
		private readonly UpgradeProgressRepositoryProvider _upgradeProgressRepositoryProvider;
		private readonly IAssetFactory _assetFactory;
		private readonly IProgressService _progressService;
		private readonly IModifiableStatsRepositoryProvider _modifiableStatsRepositoryProvider;

		private IPlayerProgressSetterFacadeProvider _playerProgressSetterFacadeProvider;

		[Inject]
		public ProgressFactory(
			IProgressSaveLoadDataService progressSaveLoadDataService,
			InitialProgressFactory initialProgressFactory,
			ProgressConstantNames progressConstantNames,
			IPersistentProgressServiceProvider persistentProgressServiceProvider,
			IPlayerProgressSetterFacadeProvider playerProgressSetterFacadeProvider,
			ISaveLoaderProvider saveLoaderProvider,
			IProgressCleaner progressCleaner,
			IResourcesProgressPresenterProvider resourcesProgressPresenterProvider,
			UpgradeProgressRepositoryProvider upgradeProgressRepositoryProvider,
			IAssetFactory assetFactory,
			IProgressService progressService,
			IModifiableStatsRepositoryProvider modifiableStatsRepositoryProvider
		)
		{
			_progressSaveLoadDataService = progressSaveLoadDataService ??
				throw new ArgumentNullException(nameof(progressSaveLoadDataService));
			_persistentProgressServiceProvider = persistentProgressServiceProvider ??
				throw new ArgumentNullException(nameof(persistentProgressServiceProvider));
			_playerProgressSetterFacadeProvider = playerProgressSetterFacadeProvider ??
				throw new ArgumentNullException(nameof(playerProgressSetterFacadeProvider));

			_progressCleaner = progressCleaner ?? throw new ArgumentNullException(nameof(progressCleaner));
			_resourcesProgressPresenterProvider = resourcesProgressPresenterProvider ??
				throw new ArgumentNullException(nameof(resourcesProgressPresenterProvider));
			_upgradeProgressRepositoryProvider = upgradeProgressRepositoryProvider ??
				throw new ArgumentNullException(nameof(upgradeProgressRepositoryProvider));
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_progressService = progressService ?? throw new ArgumentNullException(nameof(progressService));
			_modifiableStatsRepositoryProvider = modifiableStatsRepositoryProvider ??
				throw new ArgumentNullException(nameof(modifiableStatsRepositoryProvider));
		}

		public async UniTask<IGlobalProgress> Load() =>
			await _progressSaveLoadDataService.LoadFromCloud();

		public async UniTask Save(IGlobalProgress provider)
		{
			if (provider == null) throw new ArgumentNullException(nameof(provider));
			await _progressSaveLoadDataService.SaveToCloud();
		}

		public async UniTask Create()
		{
			var cloudSaves = await _progressSaveLoadDataService.LoadFromCloud();

			cloudSaves = await CreatNewIfNull(cloudSaves);

			RegisterServices(cloudSaves);
		}

		private async UniTask<IGlobalProgress> CreatNewIfNull(IGlobalProgress loadedProgress)
		{
			if (loadedProgress != null)
				return loadedProgress;

			Debug.Log("New progress model created");

			loadedProgress = await _progressCleaner.ClearAndSaveCloud();

			return loadedProgress;
		}

		private void RegisterServices(IGlobalProgress progress)
		{
			var configs = new Dictionary<int, IUpgradeEntityViewConfig>();
			var stats = new Dictionary<int, IModifiableStat>();

			IReadOnlyList<IProgressEntity> entities = progress.ShopEntity.ProgressEntities;

			UpgradeEntityListConfig configList = _assetFactory.LoadFromResources<UpgradeEntityListConfig>(
				ResourcesAssetPath.Configs.ShopItems
			);

			for (int i = 0; i < configList.ReadOnlyItems.Count(); i++)
			{
				int id = configList.ReadOnlyItems.ElementAt(i).Id;

				configs.Add(id, configList.ReadOnlyItems.ElementAt(i));
			}

			Dictionary<int, IProgressEntity> entitiesDictionary = new Dictionary<int, IProgressEntity>();
			for (int i = 0; i < entities.Count; i++)
			{
				IProgressEntity entity = entities[i];
				entitiesDictionary.Add(entity.ConfigId, entity);
			}

			_upgradeProgressRepositoryProvider.Register(
				new UpgradeProgressRepository(
					entitiesDictionary,
					configs
				)
			);

			InitModifiableStatsRepositoryProvider(progress, entities, stats, configs);

			RegisterProgressServiceProvider(new PersistentProgressService(progress));
			RegisterProgressSetterFacade();
		}

		private void InitModifiableStatsRepositoryProvider(
			IGlobalProgress progress,
			IReadOnlyList<IProgressEntity> entities,
			Dictionary<int, IModifiableStat> stats,
			Dictionary<int, IUpgradeEntityViewConfig> configs
		)
		{
			for (int i = 0; i < entities.Count(); i++)
			{
				IUpgradeEntityViewConfig config = configs.ElementAt(i).Value;

				if (config.IsModifiable == false)
					continue;

				stats.Add(
					config.Id,
					new ModifiableStat(GetProgressValue(progress, configs, i))
				);
			}

			_modifiableStatsRepositoryProvider.Register(new ModifiableStatsRepository(stats));
		}

		private int GetProgressValue(
			IGlobalProgress progress,
			Dictionary<int, IUpgradeEntityViewConfig> configs,
			int i
		)
		{
			IProgressEntity progressEntity = progress.ShopEntity.ProgressEntities[i];

			if (progressEntity.CurrentLevel - 1 >= 0 && progressEntity.CurrentLevel - 1 < configs[i].Stats.Count)
				return configs[i].Stats[progressEntity.CurrentLevel - 1];

			throw new ArgumentOutOfRangeException(
				$"Current level is {progressEntity.CurrentLevel} but it should be in range [0, {configs[i].Stats.Count}]"
			);
		}

		private void RegisterProgressSetterFacade() =>
			_playerProgressSetterFacadeProvider.Register<IProgressSetterFacade>(
				new ProgressSetterFacade(
					_progressSaveLoadDataService,
					_persistentProgressServiceProvider,
					_resourcesProgressPresenterProvider,
					_progressService
				)
			);

		private void RegisterProgressServiceProvider(IPersistentProgressService globalProgress) =>
			_persistentProgressServiceProvider.Register<IPersistentProgressService>(globalProgress);
	}
}