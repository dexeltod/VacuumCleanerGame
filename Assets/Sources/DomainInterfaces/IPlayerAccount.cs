namespace Sources.DomainInterfaces
{
	public interface IPlayerAccount
	{
		string Name { get; }
		string UniqueId { get; }
		string Language { get; }
		string ProfilePicture { get; }
	}
}