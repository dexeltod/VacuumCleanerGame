using Sources.DIService;
using Sources.Utils.Configs.Scripts;

namespace Sources.InfrastructureInterfaces.Scene
{
	public interface ILevelConfigGetter : IService
	{
		LevelConfig Get(int levelNumber);
	}
}