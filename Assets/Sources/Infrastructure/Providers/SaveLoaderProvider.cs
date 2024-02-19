using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Common.Provider;

namespace Sources.Infrastructure.Providers
{
	public sealed class SaveLoaderProvider : Provider<ISaveLoader>, ISaveLoaderProvider { }
}