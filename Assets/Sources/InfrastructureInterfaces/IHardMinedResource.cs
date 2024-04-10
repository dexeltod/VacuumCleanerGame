using UnityEngine;

namespace Sources.Infrastructure.Configs.Scripts.Level.LevelResouce
{
	public interface IHardMinedResource : IMinedResource
	{
		ParticleSystem HardResourceEffect { get; }
	}
}