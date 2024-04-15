namespace Sources.DomainInterfaces
{
	public interface IPlayerStatChangeable : IPlayerStatReadOnly
	{
		void SetValue(int value);
	}
}