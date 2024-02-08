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
			int levelIndex = level - 1;
			return _levels[levelIndex];
		}
	}
}