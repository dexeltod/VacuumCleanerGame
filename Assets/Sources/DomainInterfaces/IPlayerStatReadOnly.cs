namespace Sources.DomainInterfaces
{
	public interface IPlayerStatReadOnly : IPlayerStatSubscribable
	{
		int Value { get; }
		string Name { get; }
	}
}