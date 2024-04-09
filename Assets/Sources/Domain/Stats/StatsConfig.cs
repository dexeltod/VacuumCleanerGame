using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.Domain.Stats
{
	[Serializable] public class StatsConfig
	{
		[SerializeField] public List<Stats> Stats;
	}
}