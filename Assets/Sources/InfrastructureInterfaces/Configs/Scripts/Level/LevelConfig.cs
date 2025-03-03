using System;
using System.Collections.Generic;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces.Configs.Scripts.Level.LevelResource;
using UnityEngine;

namespace Sources.InfrastructureInterfaces.Configs.Scripts.Level
{
	[Serializable]
	public class LevelConfig
	{
		[SerializeField] private List<SoftMinedResource> _softResourcePrefab;
		[SerializeField] private List<HardMinedResource> _hardResourcesPrefab;

		[SerializeField] private List<ResourceConfig> _resourceConfigs;

		public IReadOnlyList<ISoftMinedResource> SoftMinedResource => _softResourcePrefab;
		public IReadOnlyList<IHardMinedResource> HardMinedResource => _hardResourcesPrefab;

		public IReadOnlyList<ResourceConfig> ResourceConfigs => _resourceConfigs;
	}

	[Serializable]
	public class ResourceConfig
	{
		[SerializeField] private List<GameObject> _prefabs;

		[SerializeField] private List<ScoreColor> _color;
		[SerializeField] private bool _isUniqueResource = false;

		public bool IsUniqueResource => _isUniqueResource;
		public List<ScoreColor> ScoreColor => _color;
	}

	[Serializable]
	public class ScoreColor
	{
		[SerializeField] private Material _material;
		[SerializeField] private int _score;
		[SerializeField] private Color _color;

		public int Score => _score;
		public Color Color => _color;
		public Material Material => _material;
	}
}
