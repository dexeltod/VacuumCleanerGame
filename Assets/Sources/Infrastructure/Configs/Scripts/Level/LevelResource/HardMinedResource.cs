using System;
using Sources.InfrastructureInterfaces;
using UnityEngine;

namespace Sources.Infrastructure.Configs.Scripts.Level.LevelResource
{
    [Serializable]
    public class HardMinedResource : IHardMinedResource
    {
        [SerializeField] private GameObject _prefab;

        [SerializeField] private int _score;

        [SerializeField] private Color _color;

        [SerializeField] private ParticleSystem _hardResourceEffect;

        [SerializeField] private Material _material;

        public Material Material => _material;
        public Color Color => _color;
        public GameObject Prefab => _prefab;
        public int Score => _score;
        public ParticleSystem HardResourceEffect => _hardResourceEffect;
    }
}