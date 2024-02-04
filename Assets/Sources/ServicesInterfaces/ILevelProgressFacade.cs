

namespace Sources.ServicesInterfaces
{
	public interface ILevelProgressFacade 
	{
		int CurrentLevelNumber { get; }

		void SetNextLevel();
	}
}