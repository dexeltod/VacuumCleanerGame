namespace Sources.DomainInterfaces
{
	public interface ILevelProgress
	{
		int CurrentLevel { get; }
		int MaxScoreCount { get; }

		void AddLevel(int maxPointDelta, int level);
	}
}