using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sources.Infrastructure.Configs.Scripts.Level.LevelResource;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Configs;
using UnityEngine;

namespace Sources.Infrastructure.Configs.Scripts.Level
{
	[Serializable] public class LevelConfig : ILevelConfig
	{
		[SerializeField] private List<SoftMinedResource> _softResourcePrefab;
		[SerializeField] private List<HardMinedResource> _hardResourcesPrefab;

		public IReadOnlyList<ISoftMinedResource> SoftMinedResource => _softResourcePrefab;
		public IReadOnlyList<IHardMinedResource> HardMinedResource => _hardResourcesPrefab;
	}
}