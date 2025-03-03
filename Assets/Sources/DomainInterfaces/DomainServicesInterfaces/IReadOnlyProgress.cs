namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface IReadOnlyProgress<out T> : IEventProgress
	{
		int Id { get; }
		T ReadOnlyValue { get; }
		T ReadOnlyMaxValue { get; }
		string Name { get; }
		bool IsTotalScoreReached { get; }
	}
}
