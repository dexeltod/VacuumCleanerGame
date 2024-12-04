using System;
using System.Collections.Generic;
using System.Linq;
using Sources.DomainInterfaces.Models.Shop.Upgrades;
using Sources.InfrastructureInterfaces.Configs;
using Sources.InfrastructureInterfaces.Repository;

namespace Sources.Infrastructure.Repository
{
	public sealed class ProgressEntityRepository : IProgressEntityRepository
	{
		private readonly Dictionary<int, IUpgradeEntityReadOnly> _entities;
		private readonly Dictionary<int, IUpgradeEntityViewConfig> _configs;

		public ProgressEntityRepository(
			Dictionary<int, IUpgradeEntityReadOnly> entities,
			Dictionary<int, IUpgradeEntityViewConfig> configs
		)
		{
			_entities = entities ?? throw new ArgumentNullException(nameof(entities));
			_configs = configs ?? throw new ArgumentNullException(nameof(configs));
		}

		public IUpgradeEntityReadOnly GetEntity(int id) =>
			_entities[id] ?? throw new ArgumentNullException($"{id} not found");

		public IReadOnlyList<IUpgradeEntityReadOnly> GetEntities() =>
			_entities.Values.ToList();

		public IReadOnlyList<IUpgradeEntityViewConfig> GetConfigs() =>
			_configs.Values.ToList();

		public IUpgradeEntityViewConfig GetConfig(int id) =>
			_configs[id] ?? throw new ArgumentNullException($"{id} not found");

		public int GetPrice(int id)
		{
			if (id >= _configs.Count || id < 0)
				throw new ArgumentOutOfRangeException(nameof(id));

			IUpgradeEntityViewConfig config = _configs[id];
			IUpgradeEntityReadOnly entity = _entities[id];

			if (entity.Value >= 0 && entity.Value < config.Prices.Count)
				return config.Prices[entity.Value];

			if (entity.Value - 1 >= 0 && entity.Value - 1 < config.Prices.Count)
				return config.Prices[entity.Value - 1];

			throw new ArgumentOutOfRangeException($"Value {entity.Value} out of range", nameof(id));
		}

		public float GetStatByProgress(int id)
		{
			IUpgradeEntityViewConfig config = _configs[id];

			return config.Stats[_entities[id].Value];
		}

		public void AddOneLevel(int id) =>
			(_entities[id] as IUpgradeEntity)!.AddOneLevel();
	}
}