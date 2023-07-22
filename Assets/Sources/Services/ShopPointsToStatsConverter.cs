using System;
using System.Collections.Generic;

namespace Sources.Infrastructure.Services
{
	public class ShopPointsToStatsConverter
	{
		private readonly List<int> _speedStats;
		private int _currentSpeedPoint;

		//TODO: Need to create speed balance class;
		public ShopPointsToStatsConverter(int startSpeedPoint) =>
			_speedStats = new List<int>(7) { startSpeedPoint, 5, 7, 8, 10, 12, 15 };

		public int ConvertSpeedByPoint(int currentSpeedPoint)
		{
			if (currentSpeedPoint > _speedStats.Count)
				throw new ArgumentOutOfRangeException();

			return _speedStats[currentSpeedPoint];
		}
	}
}