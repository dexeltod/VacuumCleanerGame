using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.Utils.Configs
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

	[Serializable] public class LevelConfig
	{
		[SerializeField] private string   _musicName;
		[SerializeField] private string   _levelName;
		[SerializeField] private bool     _isStopMusicBetweenScenes = false;
		[SerializeField] private int      _pointPerSand;
		[SerializeField] private Gradient _sandGradientColor;

		public bool     IsStopMusicBetweenScenes => _isStopMusicBetweenScenes;
		public string   MusicName                => _musicName;
		public string   LevelName                => _levelName;
		public int      PointPerSand             => _pointPerSand;
		public Gradient SandGradientColor        => _sandGradientColor;
	}
}