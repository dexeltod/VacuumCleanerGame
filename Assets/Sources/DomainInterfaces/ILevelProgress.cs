namespace Sources.DomainInterfaces
{
	public interface ILevelProgress
	{
		int CurrentLevel { get; }
		int MaxTotalResourceCount { get; }

		void AddLevel(int maxPointDelta, int level);
	}
}