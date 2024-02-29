using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.Domain
{
	[Serializable] public class StatsConfig
	{
		[SerializeField] public List<Stats> Stats;
	}
}