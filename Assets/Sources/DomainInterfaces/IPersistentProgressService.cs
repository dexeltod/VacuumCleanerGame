namespace Sources.DomainInterfaces
{
	public interface IPersistentProgressService
	{
		IGameProgressProvider GameProgress { get; }
	}
}