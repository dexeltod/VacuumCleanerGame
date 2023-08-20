using System;
using System.Collections.Generic;
using System.Linq;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Factory;
using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.Services
{
	public class PlayerStatsService : IPlayerStatsService
	{
		private readonly ShopPointsToStatsConverter _converter;
		private readonly List<IUpgradeProgressData> _progress;
		private readonly string[] _statNames;

		public PlayerStatsService(IPersistentProgressService progressService, IShopItemFactory itemFactory)
		{
			_progress = progressService.GameProgress.PlayerProgress.GetAll();

			_statNames = new string[_progress.Count];
			Dictionary<string, int[]> dada = new();
 
			for (int i = 0; i < _progress.Count; i++)
				_statNames[i] = _progress[i].Name;

			IReadOnlyList<int> speedStats = new List<int>(7) { 2, 5, 7, 8, 10, 12, 15 };

			// _converter = new ShopPointsToStatsConverter();
		}

		public int GetConvertedProgressValue(string progressName)
		{
			if (_statNames.Contains(progressName) == false)
				throw new InvalidOperationException("The progress name " + progressName + "not exists");

			IUpgradeProgressData progress = _progress.FirstOrDefault(elem => elem.Name == progressName);

			return _converter.GetConverted(progress.Name, progress.Value);
		}

		public void Set(string progressName, int value)
		{
			if (_statNames.Contains(progressName) == false)
				throw new InvalidOperationException("The progress name " + progressName + "not exists");

			IUpgradeProgressData changeableProgress = _progress.FirstOrDefault(elem => elem.Name == progressName);

			changeableProgress.Value = value;
		}
	}

	[Serializable]
	public class Stats
	{
		[SerializeField] public string Name;
		[SerializeField] public List<int> Points;
	}

	[Serializable]
	public class StatsConfig
	{
		[SerializeField] public List<Stats> Collection;
	}
}