using System;
using UnityEngine;

namespace Sources.Domain.Stats
{
	[Serializable] public class Stats
	{
		[SerializeField] public string Name;
		[SerializeField] public int[]  Points;
		[SerializeField] public int[]  Prices;
	}
}