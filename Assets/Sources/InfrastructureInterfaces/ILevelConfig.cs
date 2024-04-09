using System.Collections.Generic;
using Sources.Infrastructure.Configs.Scripts.Level.LevelResouce;

namespace Sources.Infrastructure.Configs.Scripts.Level
{
	public interface ILevelConfig
	{
		IReadOnlyList<ISoftMinedResource> SoftMinedResource { get; }
		IReadOnlyList<IHardMinedResource> HardMinedResource { get; }
	}
}