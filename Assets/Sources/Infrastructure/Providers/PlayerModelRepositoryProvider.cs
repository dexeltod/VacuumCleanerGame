using Sources.Infrastructure.Common.Provider;
using Sources.InfrastructureInterfaces.Providers;
using Sources.InfrastructureInterfaces.Repository;

namespace Sources.Infrastructure.Providers
{
	public sealed class
		PlayerModelRepositoryProvider : Provider<IPlayerModelRepository>, IPlayerModelRepositoryProvider { }
}