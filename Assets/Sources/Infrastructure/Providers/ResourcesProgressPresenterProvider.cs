using Sources.Infrastructure.Common.Provider;
using Sources.InfrastructureInterfaces.Presenters;
using Sources.InfrastructureInterfaces.Providers;
using Sources.ServicesInterfaces;

namespace Sources.Infrastructure.Providers
{
	public sealed class ResourcesProgressPresenterProvider : Provider<IResourcesProgressPresenter>,
		IResourcesProgressPresenterProvider { }
}