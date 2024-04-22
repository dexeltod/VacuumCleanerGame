using Sources.Infrastructure.Common.Provider;
using Sources.InfrastructureInterfaces.Providers;

namespace Sources.Infrastructure.Repositories
{
	public sealed class UpgradeProgressRepositoryProvider : Provider<IUpgradeProgressRepository>,
		IProgressEntityRepositoryProvider { }
}