using Sources.Infrastructure.Common.Provider;
using Sources.InfrastructureInterfaces.Providers;
using Sources.InfrastructureInterfaces.Repository;

namespace Sources.Infrastructure.Repository
{
	public class ModifiableStatsRepositoryProvider : Provider<IModifiableStatsRepository>,
		IModifiableStatsRepositoryProvider { }
}