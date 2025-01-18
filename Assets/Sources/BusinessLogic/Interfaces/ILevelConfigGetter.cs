using Sources.DomainInterfaces;

namespace Sources.BuisenessLogic.Interfaces
{
	public interface ILevelConfigGetter
	{
		ILevelConfig GetOrDefault(int levelNumber);
	}
}