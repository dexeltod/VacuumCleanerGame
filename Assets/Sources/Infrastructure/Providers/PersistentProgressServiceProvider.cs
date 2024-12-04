using Sources.DomainInterfaces;
using Sources.Infrastructure.Common.Provider;
using Sources.InfrastructureInterfaces.Providers;

namespace Sources.Infrastructure.Providers
{
	public class PersistentProgressServiceProvider : Provider<IPersistentProgressService>,
		IPersistentProgressServiceProvider { }
}