

namespace Sources.InfrastructureInterfaces
{
	public interface ILevelProgressFacade 
	{
		int CurrentLevelNumber { get; }

		void SetNextLevel();
	}
}