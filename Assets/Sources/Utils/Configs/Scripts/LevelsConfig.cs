using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.Utils.Configs.Scripts
{
	[CreateAssetMenu(fileName = "LevelConfigs", menuName = "Data/Level/LevelConfigs")]
	public class LevelsConfig : ScriptableObject
	{
		[SerializeField] private List<LevelConfig> _levels;

		public LevelConfig Get(int level)
		{
			if (level - 1 < 0 || level - 1 >= _levels.Count)
				throw new ArgumentOutOfRangeException($"Level {level} not found");

			return _levels[level - 1];
		}
	}
}