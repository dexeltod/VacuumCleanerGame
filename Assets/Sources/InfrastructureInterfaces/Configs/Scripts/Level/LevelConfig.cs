using System;
using System.Collections.Generic;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces.Configs.Scripts.Level.LevelResource;
using UnityEngine;

namespace Sources.InfrastructureInterfaces.Configs.Scripts.Level
{
	[Serializable]
	public class LevelConfig : ILevelConfig
	{
		[SerializeField] private List<SoftMinedResource> _softResourcePrefab;
		[SerializeField] private List<HardMinedResource> _hardResourcesPrefab;


		public IReadOnlyList<ISoftMinedResource> SoftMinedResource => _softResourcePrefab;
		public IReadOnlyList<IHardMinedResource> HardMinedResource => _hardResourcesPrefab;
	}
}
