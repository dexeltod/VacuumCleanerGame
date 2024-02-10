using Sources.Infrastructure.Common.Provider;
using Sources.InfrastructureInterfaces.Providers;
using Sources.PresentationInterfaces;

namespace Sources.Infrastructure.Providers
{
	public sealed class GameplayInterfaceProvider : Provider<IGameplayInterfaceView>, IGameplayInterfaceProvider { }
}