using UnityEngine;

namespace Sources.InfrastructureInterfaces
{
	public interface IHardMinedResource : IMinedResource
	{
		ParticleSystem HardResourceEffect { get; }
	}
}