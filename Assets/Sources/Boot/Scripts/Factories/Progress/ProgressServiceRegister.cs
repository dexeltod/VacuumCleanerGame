using System;
using System.Collections.Generic;
using Sources.BusinessLogic.Repository;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.Models.Shop.Upgrades;
using Sources.InfrastructureInterfaces.Configs;

namespace Sources.Boot.Scripts.Factories.Progress
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
			if (progress.PlayerStatsModel == null)
				throw new NullReferenceException(nameof(progress.PlayerStatsModel));
		}

		private float GetProgressStatValue(
			IGlobalProgress progress,
			Dictionary<int, IUpgradeEntityConfig> configs,
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
	}
}