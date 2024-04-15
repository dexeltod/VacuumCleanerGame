using Sources.InfrastructureInterfaces.Configs;

namespace Sources.InfrastructureInterfaces
{
	public interface ILevelConfigGetter
	{
		ILevelConfig GetOrDefault(int levelNumber);
	}
}