using Cysharp.Threading.Tasks;
using Sources.Application.StateMachine;
using Sources.Application.StateMachine.GameStates;
using Sources.DIService;
using Sources.View.SceneEntity;

namespace Sources.Application
{
	public class Game
	{
		public readonly GameStateMachine StateMachine;

		public Game(ICoroutineRunner coroutineRunner, LoadingCurtain loadingCurtain)
		{
			StateMachine = new GameStateMachine
			(
				coroutineRunner,
				new SceneLoader(),
				loadingCurtain,
				GameServices.Container
			);
		}

		public async UniTask Start() =>
			await StateMachine.Enter<InitializeServicesAndProgressState>();
	}
}