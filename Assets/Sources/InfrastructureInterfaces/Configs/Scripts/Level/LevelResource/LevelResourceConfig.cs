using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.InfrastructureInterfaces.Configs.Scripts.Level.LevelResource
{
	[Serializable]
	public class LevelResourceConfig
	{
		[SerializeField] private List<SoftMinedResource> _softResources;
		[SerializeField] private List<HardMinedResource> _hardResources;

		public IReadOnlyList<SoftMinedResource> SoftResources => _softResources;
		public IReadOnlyList<HardMinedResource> HardResources => _hardResources;
	}
}