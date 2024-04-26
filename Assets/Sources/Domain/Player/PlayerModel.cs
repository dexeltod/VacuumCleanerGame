using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Domain.Common;
using Sources.Domain.Temp;
using Sources.DomainInterfaces.Models;
using UnityEngine;

namespace Sources.Domain.Player
{
	[Serializable] public class PlayerModel : IPlayerModel
	{
		[SerializeField] private List<StatChangeable> _stats;

		public PlayerModel(List<StatChangeable> intStats) =>
			_stats = intStats ?? throw new ArgumentNullException(nameof(intStats));

		public IStatReadOnly Get(int id)
		{
			StatChangeable statChangeable = _stats.First(elem => elem.Id == id);

			if (statChangeable == null)
				throw new ArgumentOutOfRangeException(nameof(id), $"statChangeable with id {id} not found");

			return statChangeable;
		}

		public void Set(int id, float value)
		{
			StatChangeable statChangeable = _stats.First(elem => elem.Id == id);

			if (statChangeable == null)
				throw new ArgumentOutOfRangeException(nameof(id), $"statChangeable with id {id} not found");

			statChangeable.Set(value);
		}
	}
}