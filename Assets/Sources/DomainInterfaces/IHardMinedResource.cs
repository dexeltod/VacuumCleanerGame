using UnityEngine;

namespace Sources.DomainInterfaces
{
	public interface IHardMinedResource : IMinedResource
	{
		ParticleSystem HardResourceEffect { get; }
	}
}