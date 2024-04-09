using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sources.Infrastructure.Configs.Scripts.Level
{
	[CreateAssetMenu(fileName = "LevelConfigs", menuName = "Data/Level/LevelConfigs")]
	public class LevelsConfig : ScriptableObject
	{
		[SerializeField] private List<LevelConfig> _levels;

		public LevelConfig GetOrDefault(int level)
		{
			if (level - 1 < 0)
				throw new ArgumentOutOfRangeException($"Level {level} not found");

			if (level - 1 >= _levels.Count)
				return _levels.Last();

			return _levels[level - 1];
		}
	}
}