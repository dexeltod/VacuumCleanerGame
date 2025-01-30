using System;
using System.Collections.Generic;
using System.Linq;
using Sources.BusinessLogic.Configs;
using Sources.BusinessLogic.Repository;
using Sources.DomainInterfaces.Models.Shop.Upgrades;

namespace Sources.Infrastructure.Repository
{
	public sealed class ProgressEntityRepository : IProgressEntityRepository
	{
		private readonly Dictionary<int, IUpgradeEntityViewConfig> _configs;
		private readonly Dictionary<int, IStatUpgradeEntityReadOnly> _entities;

		public ProgressEntityRepository(
			Dictionary<int, IStatUpgradeEntityReadOnly> entities,
			Dictionary<int, IUpgradeEntityViewConfig> configs
		)
		{
			_entities = entities ?? throw new ArgumentNullException(nameof(entities));
			_configs = configs ?? throw new ArgumentNullException(nameof(configs));
		}

		public IStatUpgradeEntityReadOnly GetEntity(int id)
		{
			if (id < 0) throw new ArgumentOutOfRangeException(nameof(id));

			return _entities[id] ?? throw new ArgumentNullException($"Entity with id {id} not found");
		}

		public IReadOnlyList<IStatUpgradeEntityReadOnly> GetEntities() => _entities.Values.ToList();

		public IReadOnlyList<IUpgradeEntityViewConfig> GetConfigs() => _configs.Values.ToList();

		public IUpgradeEntityViewConfig GetConfig(int id) => _configs[id] ?? throw new ArgumentNullException($"{id} not found");

		public int GetPrice(int id)
		{
			if (id < 0)
				throw new ArgumentOutOfRangeException(nameof(id));

			IUpgradeEntityViewConfig viewConfig = _configs[id];
			IStatUpgradeEntityReadOnly entity = _entities[id];

			if (entity.Value >= 0 && entity.Value < viewConfig.Items.Count())
				return viewConfig.Items.ElementAt(entity.Value).Price;

			if (entity.Value - 1 >= 0 && entity.Value - 1 < viewConfig.Items.Count())
				return viewConfig.Items.ElementAt(entity.Value - 1).Price;

			throw new ArgumentOutOfRangeException($"Value {entity.Value} out of range", nameof(id));
		}

		public float GetStatByProgress(int id) =>
			_configs[id]
				.Items
				.ElementAt(_entities[id].Value).Progress;

		public void AddOneLevel(int id) => (_entities[id] as IUpgradeEntity)!.AddOneLevel();
	}
}
