using Sources.Infrastructure.Common.Provider;
using Sources.InfrastructureInterfaces.Providers;
using Sources.Utils;

namespace Sources.Infrastructure.Providers
{
	public sealed class CoroutineRunnerProvider : Provider<ICoroutineRunner>, ICoroutineRunnerProvider { }
}