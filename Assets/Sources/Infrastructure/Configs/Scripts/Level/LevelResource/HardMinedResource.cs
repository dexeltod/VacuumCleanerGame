using System;
using Sirenix.OdinInspector;
using Sources.InfrastructureInterfaces;
using UnityEngine;

namespace Sources.Infrastructure.Configs.Scripts.Level.LevelResource
{
	[Serializable] public class HardMinedResource : IHardMinedResource
	{
		[HorizontalGroup("Split", Width = 100), HideLabel, PreviewField(100), Required, AssetsOnly] [SerializeField]
		private GameObject _prefab;

		[Required] [VerticalGroup("Split/Properties")] [SerializeField]
		private int _score;

		[Required] [VerticalGroup("Split/Properties")] [SerializeField]
		private Color _color;

		[Required] [VerticalGroup("Split/Properties")] [SerializeField]
		private ParticleSystem _hardResourceEffect;

		[Required] [VerticalGroup("Split/Properties")] [SerializeField]
		private Material _material;

		public Material Material => _material;
		public Color Color => _color;
		public GameObject Prefab => _prefab;
		public int Score => _score;
		public ParticleSystem HardResourceEffect => _hardResourceEffect;
	}
}