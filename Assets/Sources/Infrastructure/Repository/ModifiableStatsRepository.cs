using System;
using System.Collections.Generic;
using Sources.Domain.Common;
using Sources.Domain.Temp;
using Sources.InfrastructureInterfaces.Repository;

namespace Sources.Infrastructure.Repository
{
	public class ModifiableStatsRepository : IModifiableStatsRepository
	{
		private readonly Dictionary<int, IStatChangeable> _stats;

		public ModifiableStatsRepository(Dictionary<int, IStatChangeable> stats) =>
			_stats = stats ?? throw new ArgumentNullException(nameof(stats));

		public void Increase(int id, int value)
		{
			Validate(id);

			_stats![id].Increase(value);
		}

		public void Decrease(int id, int value)
		{
			Validate(id);

			if (_stats != null)
				_stats[id].Decrease(value);
			else
				throw new ArgumentNullException($"stat with id {id} not found");
		}

		public void Clear(int id)
		{
			Validate(id);

			if (_stats != null) _stats[id].Clear();
			else
				throw new ArgumentNullException($"stat with id {id} not found");
		}

		public IStatChangeable Get(int id)
		{
			Validate(id);

			return _stats[id] ?? throw new ArgumentNullException($"stat with id {id} not found");
		}

		private void Validate(int id)
		{
			if (_stats != null && _stats[id] is not StatChangeable)
				throw new ArgumentException($"stat {_stats[id].GetType()} is not modifiable");

			if (_stats != null && _stats.ContainsKey(id) == false)
				throw new ArgumentNullException($"stat with id {id} not found");
		}
	}
}