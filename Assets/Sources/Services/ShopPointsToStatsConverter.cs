using System;
using System.Collections.Generic;
using System.Linq;

namespace Sources.Services
{
	public class ShopPointsToStatsConverter
	{
		private readonly List<int> _speedStats;
		private readonly Dictionary<string, int[]> _stats;

		private int _currentSpeedPoint;

		public ShopPointsToStatsConverter(Dictionary<string, int[]> stats)
		{
			_stats = stats;
		}

		public int GetConverted(string name, int value)
		{
			if (_stats.ContainsKey(name) == false)
				throw new ArgumentException($"Stat {name} is not existing");

			int[] convertedValues = _stats.FirstOrDefault(element => element.Key == name).Value;

			if (value < 0 || value > convertedValues.Length)
				throw new ArgumentOutOfRangeException("Value out of range for " + name);

			return convertedValues[value];
		}
	}
}