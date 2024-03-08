

namespace Sources.ServicesInterfaces
{
	public interface ILevelProgressFacade 
	{
		int CurrentLevel { get; }
		int MaxScoreCount { get; }
		void SetNextLevel();
	}
}