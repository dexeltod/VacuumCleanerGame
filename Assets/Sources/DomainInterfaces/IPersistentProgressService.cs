namespace Sources.DomainInterfaces
{
	public interface IPersistentProgressService
	{
		IGlobalProgress GlobalProgress { get; }
	}
}