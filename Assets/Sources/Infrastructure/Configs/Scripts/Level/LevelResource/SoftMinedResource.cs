using System;
using UnityEngine;

namespace Sources.Infrastructure.Configs.Scripts.Level.LevelResouce
{
	[Serializable] public class SoftMinedResource : ISoftMinedResource
	{
		[SerializeField] private GameObject _prefab;
		[SerializeField] private int _score;
		[SerializeField] private Color _color;
		[SerializeField] private Material _material;

		public Material Material => _material;
		public Color Color => _color;
		public GameObject Prefab => _prefab;
		public int Score => _score;
	}
}