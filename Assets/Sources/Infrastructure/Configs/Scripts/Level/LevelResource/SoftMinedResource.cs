using System;
using Sources.InfrastructureInterfaces;
using Sources.Presentation.SceneEntity;
using UnityEngine;

namespace Sources.Infrastructure.Configs.Scripts.Level.LevelResource
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

        private void AddPresentationIfNull()
        {
            if (_prefab.TryGetComponent<ResourcePresentation>(out _) == false)
                _prefab.AddComponent<ResourcePresentation>();
        }
    }
}