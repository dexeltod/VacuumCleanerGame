using System;
using Sources.DomainInterfaces;
using UnityEngine;

namespace Sources.InfrastructureInterfaces.Configs.Scripts.Level.LevelResource
{
	[Serializable]
	public class SoftMinedResource : ISoftMinedResource
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
