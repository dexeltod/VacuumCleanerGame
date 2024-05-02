using System;
using Sirenix.OdinInspector;
using Sources.InfrastructureInterfaces;
using Sources.Presentation.SceneEntity;
using UnityEngine;

namespace Sources.Infrastructure.Configs.Scripts.Level.LevelResource
{
	[Serializable] public class SoftMinedResource : ISoftMinedResource
	{
		[HorizontalGroup("Split", Width = 50), PreviewField, HideLabel,
		 Required, OnValueChanged("AddPresentationIfNull"), AssetsOnly, SerializeField]
		private GameObject _prefab;

		[Required] [VerticalGroup("Split/Properties")] [SerializeField]
		private int _score;

		[Required] [VerticalGroup("Split/Properties")] [SerializeField]
		private Color _color;

		[Required] [VerticalGroup("Split/Properties")] [SerializeField]
		private Material _material;

		public Material Material => _material;
		public Color Color => _color;
		public GameObject Prefab => _prefab;
		public int Score => _score;

		private void AddPresentationIfNull()
		{
			if (_prefab.TryGetComponent<ResourcePresentation>(out _) == false)
				_prefab.AddComponent<ResourcePresentation>();
		}
	}
}