
using Sources.Utils.Configs.Scripts;

namespace Sources.ServicesInterfaces
{
	public interface ILevelConfigGetter 
	{
		LevelConfig Get(int levelNumber);
	}
}