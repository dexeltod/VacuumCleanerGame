using Sources.ControllersInterfaces;
using Sources.Infrastructure.Common.Provider;
using Sources.InfrastructureInterfaces.Providers;

namespace Sources.Infrastructure.Providers
{
	public sealed class FillMeshShaderControllerProvider : Provider<IFillMeshShaderController>,
		IFillMeshShaderControllerProvider { }
}