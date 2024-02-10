using Sources.ControllersInterfaces;
using Sources.Infrastructure.Common.Provider;
using Sources.InfrastructureInterfaces.Providers;

namespace Sources.Infrastructure.Providers
{
	public sealed class ResourcesProgressPresenterProvider : Provider<IResourcesProgressPresenter>,
		IResourcesProgressPresenterProvider { }
}