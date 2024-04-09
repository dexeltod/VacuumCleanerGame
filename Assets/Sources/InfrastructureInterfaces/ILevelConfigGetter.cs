using Sources.Infrastructure.Configs.Scripts.Level;

namespace Sources.ServicesInterfaces
{
	public interface ILevelConfigGetter
	{
		ILevelConfig GetOrDefault(int levelNumber);
	}
}