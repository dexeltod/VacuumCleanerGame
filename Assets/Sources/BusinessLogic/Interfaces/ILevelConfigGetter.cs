using Sources.InfrastructureInterfaces.Configs.Scripts.Level;

namespace Sources.BusinessLogic.Interfaces
{
	public interface ILevelConfigGetter
	{
		LevelConfig GetOrDefault(int levelNumber);
		LevelsConfig GetOrDefault();
	}
}
