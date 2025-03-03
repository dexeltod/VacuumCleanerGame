using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.InfrastructureInterfaces.Configs.Scripts.Level
{
	[Serializable]
	public class LevelConfig
	{
		[SerializeField] private ResourcesConfig _resourcesConfig;
		[SerializeField] private int _hardResourceCount;

		public ResourcesConfig ResourcesConfig => _resourcesConfig;
		public int PrefabsCount => _resourcesConfig.Prefabs.Count;
		public int HardResourceCount => _hardResourceCount;
	}

	[Serializable]
	public class ResourcesConfig
	{
		[SerializeField] private List<GameObject> _prefabs;

		[SerializeField] private List<ScoreColor> _color;
		[SerializeField] private bool _isUniqueResource;

		public bool IsUniqueResource => _isUniqueResource;
		public List<ScoreColor> ScoreColor => _color;
		public List<GameObject> Prefabs => _prefabs;
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
