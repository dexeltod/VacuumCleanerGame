using System;
using UnityEngine;

namespace Sources.Utils.Configs.Scripts
{
	[Serializable] public class LevelConfig
	{
		[SerializeField] private string _musicName;
		[SerializeField] private string _levelName = "Game";
		[SerializeField] private bool _isStopMusicBetweenScenes = false;
		[SerializeField] private int _pointPerSand = 1;
		[SerializeField] private Gradient _sandGradientColor;

		public bool IsStopMusicBetweenScenes => _isStopMusicBetweenScenes;
		public string MusicName => _musicName;
		public string LevelName => _levelName;
		public int PointPerSand => _pointPerSand;
		public Gradient SandGradientColor => _sandGradientColor;
	}
}