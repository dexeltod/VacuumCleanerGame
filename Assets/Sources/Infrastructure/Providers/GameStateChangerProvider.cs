using Sources.Infrastructure.Common.Provider;
using Sources.InfrastructureInterfaces.Providers;
using Sources.InfrastructureInterfaces.Services;

namespace Sources.Infrastructure.Providers
{
	public sealed class GameStateChangerProvider : Provider<IGameStateChanger>, IGameStateChangerProvider { }
}