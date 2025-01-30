using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Domain.Stats;
using Sources.DomainInterfaces.Entities;
using Sources.DomainInterfaces.Models;
using UnityEngine;

namespace Sources.Domain.Player
{
	[Serializable]
	public class PlayerStatsModel : IPlayerStatsModel
	{
		[SerializeField] private List<Stat> _stats;

		public PlayerStatsModel(List<Stat> intStats) => _stats = intStats ?? throw new ArgumentNullException(nameof(intStats));

		public IStatReadOnly Get(int id)
		{
			if (id < 0) throw new ArgumentOutOfRangeException(nameof(id));

			Stat stat = _stats.FirstOrDefault(elem => elem.Id == id)
			            ?? throw new ArgumentException($"stat with id {id} is not exist");

			return stat;
		}

		public void Set(int id, float value)
		{
			if (id < 0) throw new ArgumentOutOfRangeException(nameof(id));

			Stat stat = _stats.First(elem => elem.Id == id);

			if (stat == null)
				throw new ArgumentOutOfRangeException(nameof(id), $"stat with id {id} not found");

			stat.Set(value);
		}
	}
}
