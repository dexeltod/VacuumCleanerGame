using System;
using System.Collections.Generic;
using System.Linq;
using Sources.DomainInterfaces.Models.Shop.Upgrades;
using Sources.InfrastructureInterfaces.Configs;
using Sources.InfrastructureInterfaces.Repository;

namespace Sources.Infrastructure.Repository
{
	public sealed class UpgradeProgressRepository : IUpgradeProgressRepository
	{
		private readonly Dictionary<int, IUpgradeEntityReadOnly> _entities;
		private readonly Dictionary<int, IUpgradeEntityViewConfig> _configs;

		public UpgradeProgressRepository(
			Dictionary<int, IUpgradeEntityReadOnly> entities,
			Dictionary<int, IUpgradeEntityViewConfig> configs
		)
		{
			_entities = entities ?? throw new ArgumentNullException(nameof(entities));
			_configs = configs ?? throw new ArgumentNullException(nameof(configs));
		}

		public IUpgradeEntityReadOnly GetEntity(int id) =>
			_entities[id] ??
			throw new ArgumentNullException($"{id} not found");

		public IReadOnlyList<IUpgradeEntityReadOnly> GetEntities() =>
			_entities.Values.ToList();

		public IReadOnlyList<IUpgradeEntityViewConfig> GetConfigs() =>
			_configs.Values.ToList();

		public IUpgradeEntityViewConfig GetConfig(int id) =>
			_configs[id] ??
			throw new ArgumentNullException($"{id} not found");

		public int GetPrice(int id)
		{
			IUpgradeEntityViewConfig config = _configs[id];
			IUpgradeEntityReadOnly entity = _entities[id];

			return config.Prices[entity.Value];
		}

		public float GetStatByProgress(int id)
		{
			IUpgradeEntityViewConfig config = _configs[id];

			int currentLevel = _entities[id].Value;

			return config.Stats[currentLevel];
		}

		public void AddOneLevel(int id) =>
			(_entities[id] as IUpgradeEntity)!.AddOneLevel();
	}
}