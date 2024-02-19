using Sources.Infrastructure.Common.Provider;
using Sources.InfrastructureInterfaces.Providers;
using Sources.ServicesInterfaces;

namespace Sources.Infrastructure.Providers
{
	public sealed class PlayerProgressSetterFacadeProvider : Provider<IProgressSetterFacade>,
		IPlayerProgressSetterFacadeProvider { }
}