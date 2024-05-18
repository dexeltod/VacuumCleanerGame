using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sources.Domain.Player;
using Sources.Domain.Progress;
using Sources.Domain.Progress.Entities.Values;
using Sources.Domain.Progress.Player;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.DomainInterfaces.Models.Shop.Upgrades;
using Sources.Infrastructure.DataViewModel;
using Sources.Infrastructure.Factories.UpgradeEntitiesConfigs;
using Sources.Infrastructure.Providers;
using Sources.Infrastructure.Repository;
using Sources.InfrastructureInterfaces.Configs;
using Sources.InfrastructureInterfaces.Providers;
using Sources.InfrastructureInterfaces.Repository;
using Sources.InfrastructureInterfaces.Services;
using Sources.Services.DomainServices;
using Sources.ServicesInterfaces;
using Sources.Utils;
using UnityEngine;
using VContainer;

namespace Sources.Infrastructure.Factories
{
	[Serializable] public class ProgressFactory
	{
		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
		private readonly IPersistentProgressServiceProvider _persistentProgressServiceProvider;
		private readonly IProgressCleaner _progressCleaner;
		private readonly UpgradeProgressRepositoryProvider _upgradeProgressRepositoryProvider;
		private readonly IAssetFactory _assetFactory;
		private readonly IPlayerModelRepositoryProvider _playerModelRepositoryProvider;

		private readonly ISaveLoaderProvider _saveLoaderProvider;

		[Inject]
		public ProgressFactory(
			IProgressSaveLoadDataService progressSaveLoadDataService,
			IPersistentProgressServiceProvider persistentProgressServiceProvider,
			ISaveLoaderProvider saveLoaderProvider,
			IProgressCleaner progressCleaner,
			UpgradeProgressRepositoryProvider upgradeProgressRepositoryProvider,
			IAssetFactory assetFactory,
			IPlayerModelRepositoryProvider playerModelRepositoryProvider
		)
		{
			_progressSaveLoadDataService = progressSaveLoadDataService ??
				throw new ArgumentNullException(nameof(progressSaveLoadDataService));
			_persistentProgressServiceProvider = persistentProgressServiceProvider ??
				throw new ArgumentNullException(nameof(persistentProgressServiceProvider));
			_saveLoaderProvider = saveLoaderProvider ?? throw new ArgumentNullException(nameof(saveLoaderProvider));

			_progressCleaner = progressCleaner ?? throw new ArgumentNullException(nameof(progressCleaner));
			_upgradeProgressRepositoryProvider = upgradeProgressRepositoryProvider ??
				throw new ArgumentNullException(nameof(upgradeProgressRepositoryProvider));
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_playerModelRepositoryProvider = playerModelRepositoryProvider ??
				throw new ArgumentNullException(nameof(playerModelRepositoryProvider));
		}

		public async UniTask Create()
		{
			IGlobalProgress cloudSaves = await _progressSaveLoadDataService.LoadFromCloud();

			Debug.Log("Loaded from cloud");
			cloudSaves = await CreatNewIfNull(cloudSaves);

			RegisterServices(cloudSaves);
		}

		private async UniTask<IGlobalProgress> CreatNewIfNull(IGlobalProgress loadedProgress)
		{
			Debug.Log($"Loaded progress is {loadedProgress}");

			if (loadedProgress != null && loadedProgress.Validate())
			{
				Debug.Log("Loaded progress is valid");
				return loadedProgress;
			}

			Debug.Log("New progress creating");

			loadedProgress = _progressCleaner.ClearAndSaveCloud();

			await _saveLoaderProvider.Self.Save(loadedProgress);
			return loadedProgress;
		}

		private void RegisterServices(IGlobalProgress progress)
		{
			if (progress.PlayerModel == null)
				throw new NullReferenceException(nameof(progress.PlayerModel));

			_playerModelRepositoryProvider.Register(new PlayerModelRepository(progress.PlayerModel));

			var configs = new Dictionary<int, IUpgradeEntityViewConfig>();

			var entities = progress.ShopModel.ProgressEntities;

			UpgradeEntityListConfig configList = _assetFactory.LoadFromResources<UpgradeEntityListConfig>(
				ResourcesAssetPath.Configs.ShopItems
			);

			for (int i = 0; i < configList.ReadOnlyItems.Count(); i++)
			{
				int id = configList.ReadOnlyItems.ElementAt(i).Id;

				configs.Add(id, configList.ReadOnlyItems.ElementAt(i));
			}

			Debug.Log("register upgrade progress repository provider");
			RegisterUpgradeProgressRepositoryProvider(entities, configs);
			Debug.Log("register progress service provider");
			RegisterProgressServiceProvider(new PersistentProgressService(progress));
			Debug.Log("registering is done");
		}

		private void RegisterUpgradeProgressRepositoryProvider(
			IEnumerable<IUpgradeEntityReadOnly> entities,
			Dictionary<int, IUpgradeEntityViewConfig> configs
		)
		{
			if (entities == null) throw new ArgumentNullException(nameof(entities));
			if (configs == null) throw new ArgumentNullException(nameof(configs));

			_upgradeProgressRepositoryProvider.Register(
				new UpgradeProgressRepository(
					entities.ToDictionary(entity => entity.ConfigId),
					configs
				)
			);
		}

		private float GetProgressStatValue(
			IGlobalProgress progress,
			Dictionary<int, IUpgradeEntityViewConfig> configs,
			int i
		)
		{
			IUpgradeEntityReadOnly upgradeEntity = progress.ShopModel.ProgressEntities[i];

			if (upgradeEntity.ConfigId >= 0 && upgradeEntity.ConfigId < configs.Count)
				return configs[upgradeEntity.ConfigId].Stats[upgradeEntity.Value];

			throw new ArgumentOutOfRangeException(
				$"Config id is {upgradeEntity.ConfigId} but it should be in range [0, {configs[i].Stats.Count}]"
			);
		}

		private void RegisterProgressServiceProvider(IPersistentProgressService globalProgress) =>
			_persistentProgressServiceProvider.Register<IPersistentProgressService>(globalProgress);
	}
}