using System;
using System.Collections.Generic;
using System.Linq;
using Sources.BusinessLogic.Repository;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.Models.Shop.Upgrades;
using Sources.Infrastructure.Factories.UpgradeEntitiesConfigs;
using Sources.Infrastructure.Repository;
using Sources.Infrastructure.Services.DomainServices;
using Sources.InfrastructureInterfaces.Configs;
using Sources.Utils;

namespace Sources.Infrastructure.Factories.Progress
{
	public class ProgressServiceRegister
	{
		private readonly IPersistentProgressService _persistentProgressServiceProvider;
		private readonly IProgressEntityRepository _progressEntityRepository;
		private readonly IAssetFactory _assetFactory;
		private readonly IPlayerModelRepository _playerModelRepositoryProvider;

		public ProgressServiceRegister(
			IPersistentProgressService persistentProgressServiceProvider,
			IProgressEntityRepository progressEntityRepositoryProvider,
			IAssetFactory assetFactory
		)
		{
			_persistentProgressServiceProvider = persistentProgressServiceProvider ??
			                                     throw new ArgumentNullException(nameof(persistentProgressServiceProvider));
			_progressEntityRepository = progressEntityRepositoryProvider ??
			                            throw new ArgumentNullException(nameof(progressEntityRepositoryProvider));
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
		}

		public void Do(IGlobalProgress progress)
		{
			if (progress.PlayerModel == null)
				throw new NullReferenceException(nameof(progress.PlayerModel));

			var configs = new Dictionary<int, IUpgradeEntityViewConfig>();

			IReadOnlyList<IStatUpgradeEntityReadOnly> entities = progress.ShopModel.ProgressEntities;

			var configList = _assetFactory.LoadFromResources<UpgradesListConfig>(
				ResourcesAssetPath.Configs.ShopItems
			);

			for (var i = 0; i < configList.ReadOnlyItems.Count(); i++)
			{
				int id = configList.ReadOnlyItems.ElementAt(i).Id;
				IUpgradeEntityViewConfig a = configList.ReadOnlyItems.ElementAt(i);

				configs.Add(id, configList.ReadOnlyItems.ElementAt(i));
			}

			RegisterUpgradeProgressRepositoryProvider(entities, configs);

			RegisterProgressServiceProvider(new PersistentProgressService(progress));
		}

		private void RegisterUpgradeProgressRepositoryProvider(
			Dictionary<int, IStatUpgradeEntityReadOnly> entities,
			Dictionary<int, IUpgradeEntityViewConfig> configs
		)
		{
			if (entities == null) throw new ArgumentNullException(nameof(entities));
			if (configs == null) throw new ArgumentNullException(nameof(configs));

			_progressEntityRepository.Register(
				new ProgressEntityRepository(
					entities,
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
			IStatUpgradeEntityReadOnly statUpgradeEntity = progress.ShopModel.ProgressEntities[i];

			if (statUpgradeEntity.ConfigId >= 0 && statUpgradeEntity.ConfigId < configs.Count)
				return configs[statUpgradeEntity.ConfigId].Stats[statUpgradeEntity.Value];

			throw new ArgumentOutOfRangeException(
				$"Config id is {statUpgradeEntity.ConfigId} but it should be in range [0, {configs[i].Stats.Count}]"
			);
		}

		private void RegisterProgressServiceProvider(IPersistentProgressService globalProgress) =>
			_persistentProgressServiceProvider.Register<IPersistentProgressService>(globalProgress);
	}
}
