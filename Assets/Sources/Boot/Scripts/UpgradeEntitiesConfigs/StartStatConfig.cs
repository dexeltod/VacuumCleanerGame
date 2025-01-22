using System;
using Sources.Utils.Enums;
using UnityEngine;

namespace Sources.Boot.Scripts.UpgradeEntitiesConfigs
{
	[Serializable]
	public class StartStatConfig
	{
		[SerializeField] private float _stat;
		[SerializeField] private ProgressType _type;

		public ProgressType Type => _type;
		public int Id => (int)_type;

		public float Stat => _stat;
	}
}