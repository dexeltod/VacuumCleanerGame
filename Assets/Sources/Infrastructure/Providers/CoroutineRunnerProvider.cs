using Sources.Infrastructure.Common.Provider;
using Sources.UseCases.Scene;

namespace Sources.Infrastructure.StateMachine.GameStates
{
	public sealed class CoroutineRunnerProvider : Provider<ICoroutineRunner>, ICoroutineRunnerProvider { }
}