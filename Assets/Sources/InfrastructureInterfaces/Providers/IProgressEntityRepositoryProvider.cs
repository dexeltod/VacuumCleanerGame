using Sources.Infrastructure.Repositories;
using Sources.InfrastructureInterfaces.Common.Providers;

namespace Sources.InfrastructureInterfaces.Providers
{
	public interface IProgressEntityRepositoryProvider : IProvider<IUpgradeProgressRepository> { }
}