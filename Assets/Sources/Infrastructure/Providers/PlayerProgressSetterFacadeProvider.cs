using Sources.Infrastructure.Common.Provider;
using Sources.InfrastructureInterfaces.Providers;
using Sources.ServicesInterfaces.DTO;

namespace Sources.Infrastructure.Providers
{
	public sealed class PlayerProgressSetterFacadeProvider : Provider<IPlayerProgressSetterFacade>,
		IPlayerProgressSetterFacadeProvider { }
}