using System.Collections.Generic;

namespace Sources.InfrastructureInterfaces.Configs
{
	public interface ILevelConfig
	{
		IReadOnlyList<ISoftMinedResource> SoftMinedResource { get; }
		IReadOnlyList<IHardMinedResource> HardMinedResource { get; }
	}
}