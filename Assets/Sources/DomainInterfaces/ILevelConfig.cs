using System.Collections.Generic;

namespace Sources.DomainInterfaces
{
	public interface ILevelConfig
	{
		IReadOnlyList<ISoftMinedResource> SoftMinedResource { get; }
		IReadOnlyList<IHardMinedResource> HardMinedResource { get; }
	}
}