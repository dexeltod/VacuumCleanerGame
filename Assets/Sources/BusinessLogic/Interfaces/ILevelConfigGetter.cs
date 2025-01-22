using Sources.DomainInterfaces;

namespace Sources.BusinessLogic.Interfaces
{
	public interface ILevelConfigGetter
	{
		ILevelConfig GetOrDefault(int levelNumber);
	}
}