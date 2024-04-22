using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Domain.Temp;
using Sources.InfrastructureInterfaces.Configs;

namespace Sources.Infrastructure.Repositories
{
	public sealed class UpgradeProgressRepository : IUpgradeProgressRepository
	{
		private readonly Dictionary<int, IProgressEntity> _entities;
		private readonly Dictionary<int, IUpgradeEntityViewConfig> _configs;

		public UpgradeProgressRepository(
			Dictionary<int, IProgressEntity> entities,
			Dictionary<int, IUpgradeEntityViewConfig> configs
		)
		{
			_entities = entities ?? throw new ArgumentNullException(nameof(entities));
			_configs = configs ?? throw new ArgumentNullException(nameof(configs));
		}

		public IProgressEntity GetEntity(int id) =>
			_entities[id] ??
			throw new ArgumentNullException($"{id} not found");

		public IReadOnlyList<IProgressEntity> GetEntities() =>
			_entities.Values.ToList();

		public IReadOnlyList<IUpgradeEntityViewConfig> GetConfigs() =>
			_configs.Values.ToList();

		public IUpgradeEntityViewConfig GetConfig(int id) =>
			_configs[id] ??
			throw new ArgumentNullException($"{id} not found");

		public int GetPrice(int id)
		{
			IUpgradeEntityViewConfig config = _configs[id];
			IProgressEntity entity = _entities[id];

			return config.Prices[entity.CurrentLevel - 1];
		}

		public int GetStatByProgress(int id)
		{
			IUpgradeEntityViewConfig config = _configs[id];

			int currentLevel = _entities[id].CurrentLevel;

			return config.Stats[currentLevel - 1];
		}

		public void AddOneLevel(int id) =>
			_entities[id].AddOneLevel();
	}
}