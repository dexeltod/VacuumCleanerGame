using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sources.InfrastructureInterfaces.Configs.Scripts.Level
{
	[CreateAssetMenu(fileName = "LevelConfigs", menuName = "Data/Level/LevelConfigs")]
	public class LevelsConfig : ScriptableObject
	{
		[SerializeField] private List<LevelConfig> _levels;
		[SerializeField] private ParticleSystem _hardEffect;

		public ParticleSystem HardEffectParticle => _hardEffect;

		public LevelConfig GetOrDefault(int level)
		{
			if (level - 1 < 0)
				throw new ArgumentOutOfRangeException($"Level {level} not found");

			return level - 1 >= _levels.Count ? _levels.Last() : _levels[level - 1];
		}
	}
}
