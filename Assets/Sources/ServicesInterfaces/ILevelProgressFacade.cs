namespace Sources.ServicesInterfaces
{
	public interface ILevelProgressFacade
	{
		int CurrentLevel { get; }
		int MaxTotalResourceCount { get; }
		void SetNextLevel();
	}
}