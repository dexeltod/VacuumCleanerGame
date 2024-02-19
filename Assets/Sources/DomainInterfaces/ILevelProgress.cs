namespace Sources.DomainInterfaces
{
	public interface ILevelProgress
	{
		int CurrentLevel { get; }
		void AddLevel(int level);
	}
}