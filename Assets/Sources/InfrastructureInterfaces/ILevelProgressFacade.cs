using Sources.DIService;

namespace Sources.InfrastructureInterfaces
{
	public interface ILevelProgressFacade : IService
	{
		int CurrentLevelNumber { get; }

		void SetNextLevel();
	}
}