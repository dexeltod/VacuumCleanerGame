using System;
using System.Collections.Generic;
using System.Linq;
using Sources.DomainServices.Interfaces;
using Sources.Services;

namespace Sources.Infrastructure.Services
{
	public class PlayerStats : IPlayerStatsService
	{
		private const int StartPlayerSpeed = 4;

		private readonly ShopPointsToStatsConverter _converter;
		private readonly Dictionary<string, IStat> _stats;

		private bool _isInit = false;
		public int Speed { get; private set; }
		public int VacuumDistance { get; private set; }

		public PlayerStats(IPersistentProgressService progressService)
		{
			List<Tuple<string, int>> progress = progressService.GameProgress.PlayerProgress.GetAll();
			
			_stats = new();

			foreach (var element in progress)
			{
				
				_stats.Add(element.Item1, element.Item2);
			} 

			_converter = new ShopPointsToStatsConverter(_stats);
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
			if (_stats.ContainsKey(progressName) == false)
				throw new InvalidOperationException("The progress name " + progressName + "not exists");
			
			KeyValuePair<string, int> progress = _stats.FirstOrDefault(elem => elem.Key == progressName);
			
			_converter.Convert(progress);
		}

		private void CheckNames(string name, int value)
		{
			if (name == "Speed")
				Speed = _converter.ConvertSpeedByPoint(value);
			else if (name == "Radius")
				VacuumDistance = value;
		}
	}
}