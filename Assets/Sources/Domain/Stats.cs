using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.Domain
{
	[Serializable] public class Stats
	{
		[SerializeField] public string Name;
		[SerializeField] public int[]  Points;
		[SerializeField] public int[]  Prices;
	}
}