using System;
using System.Collections.Generic;
using Sources.Core;

namespace Sources.Infrastructure.Services
{
	public class PlayerStats : IPlayerStatsService
	{
		private const int StartPlayerSpeed = 4;

		private readonly ShopPointsToStatsConverter _converter;
		private bool _isInit = false;
		public int Speed { get; private set; }
		public int VacuumDistance { get; private set; }

		public PlayerStats()
		{
			_converter = new ShopPointsToStatsConverter(StartPlayerSpeed);
		}

		public void Initialize(List<Tuple<string, int>> stats)
		{
			if (_isInit)
				return;

			foreach (var item in stats)
				CheckNames(item.Item1, item.Item2);

			_isInit = false;
		}

		public void Set(string progressName, int value)
		{
			CheckNames(progressName, value);
		}

		private void CheckNames(string name, int value)
		{
			if (name == "Speed")
				Speed = _converter.ConvertSpeedByPoint(value);
			else if (name == "Radius")
				VacuumDistance = value;
		}
	}

	public interface IPlayerStatsService : IService
	{
		public int Speed { get; }
		public int VacuumDistance { get; }

		void Set(string name, int value);

		public void Initialize(List<Tuple<string, int>> stats);
	}
}