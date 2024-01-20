
using Sources.Utils.Configs.Scripts;

namespace Sources.InfrastructureInterfaces.Scene
{
	public interface ILevelConfigGetter 
	{
		LevelConfig Get(int levelNumber);
	}
}