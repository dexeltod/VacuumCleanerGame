using Sources.DIService;
using Sources.Utils.Configs;

namespace Sources.InfrastructureInterfaces.Scene
{
	public interface ILevelConfigGetter : IService
	{
		LevelConfig Get(int levelNumber);
	}
}