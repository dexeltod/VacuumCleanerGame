using System;
using UnityEngine;

namespace Sources.Infrastructure.Configs.Scripts.Level.LevelResouce
{
	[Serializable] public class HardMinedResource : IHardMinedResource
	{
		[SerializeField] private GameObject _prefab;
		[SerializeField] private int _score;
		[SerializeField] private Color _color;

		public Color Color => _color;
		public GameObject Prefab => _prefab;
		public int Score => _score;
	}
}