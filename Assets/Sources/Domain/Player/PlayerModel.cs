using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Domain.Stats;
using Sources.DomainInterfaces.Entities;
using Sources.DomainInterfaces.Models;
using UnityEngine;

namespace Sources.Domain.Player
{
	[Serializable] public class PlayerModel : IPlayerModel
	{
		[SerializeField] private List<Stat> _stats;

		public PlayerModel(List<Stat> intStats) =>
			_stats = intStats ?? throw new ArgumentNullException(nameof(intStats));

		public IStatReadOnly Get(int id)
		{
			Stat stat = _stats.First(elem => elem.Id == id);

			if (stat == null)
				throw new ArgumentOutOfRangeException(nameof(id), $"stat with id {id} not found");

			return stat;
		}

		public void Set(int id, float value)
		{
			Stat stat = _stats.First(elem => elem.Id == id);

			if (stat == null)
				throw new ArgumentOutOfRangeException(nameof(id), $"stat with id {id} not found");

			stat.Set(value);
		}
	}
}