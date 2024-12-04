using System;
using System.Collections.Generic;
using System.Linq;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.Models.Shop.Upgrades;
using Sources.Infrastructure.Factories.UpgradeEntitiesConfigs;
using Sources.Infrastructure.Repository;
using Sources.InfrastructureInterfaces.Configs;
using Sources.InfrastructureInterfaces.Providers;
using Sources.Services.DomainServices;
using Sources.ServicesInterfaces;
using Sources.Utils;
using UnityEngine;
using VContainer;

namespace Sources.Infrastructure.Factories
{
	public class ProgressServiceRegister
	{
		private readonly IPersistentProgressServiceProvider _persistentProgressServiceProvider;
		private readonly IProgressEntityRepositoryProvider _progressEntityRepositoryProvider;
		private readonly IAssetFactory _assetFactory;
		private readonly IPlayerModelRepositoryProvider _playerModelRepositoryProvider;

		[Inject]
		public ProgressServiceRegister(
			IPersistentProgressServiceProvider persistentProgressServiceProvider,
			IProgressEntityRepositoryProvider progressEntityRepositoryProvider,
			IAssetFactory assetFactory,
			IPlayerModelRepositoryProvider playerModelRepositoryProvider
		)
		{
			_persistentProgressServiceProvider = persistentProgressServiceProvider ??
				throw new ArgumentNullException(nameof(persistentProgressServiceProvider));
			_progressEntityRepositoryProvider = progressEntityRepositoryProvider ??
				throw new ArgumentNullException(nameof(progressEntityRepositoryProvider));
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_playerModelRepositoryProvider = playerModelRepositoryProvider ??
				throw new ArgumentNullException(nameof(playerModelRepositoryProvider));
		}

		public void Do(IGlobalProgress progress)
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

			_progressEntityRepositoryProvider.Register(
				new ProgressEntityRepository(
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