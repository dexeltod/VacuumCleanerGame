using Sources.Infrastructure.Common.Provider;
using Sources.InfrastructureInterfaces.Providers;
using Sources.ServicesInterfaces;

namespace Sources.Infrastructure.Providers
{
	public sealed class PlayerStatsServiceProvider : Provider<IPlayerStatsService>, IPlayerStatsServiceProvider { }
}