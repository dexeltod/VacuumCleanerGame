
using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.DomainInterfaces
{
	public interface IPersistentProgressService : IPersistentProgressServiceConstructable
	{
		IGameProgressModel GameProgress { get; }
	}
}