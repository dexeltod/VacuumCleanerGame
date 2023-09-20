using Sources.DIService;
using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.DomainInterfaces
{
	public interface IPersistentProgressService : IPersistentProgressServiceConstructable, IService
	{
		IGameProgressModel GameProgress { get; }
	}
}