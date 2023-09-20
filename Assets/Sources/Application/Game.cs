using Cysharp.Threading.Tasks;
using Sources.Application.StateMachine;
using Sources.Application.StateMachine.GameStates;
using Sources.DIService;
using Sources.PresentationInterfaces;
using Sources.View.SceneEntity;

namespace Sources.Application
{
	public class Game
	{
		public readonly GameStateMachine StateMachine;

		public Game
		(
			ICoroutineRunner coroutineRunner,
			LoadingCurtain loadingCurtain,
			IYandexAuthorizationHandler yandexAuthorizationHandler
		)
		{
			StateMachine = new GameStateMachine
			(new SceneLoader(),
				loadingCurtain,
				yandexAuthorizationHandler,
				GameServices.Container
			);
		}

		public void Start() =>
			StateMachine.Enter<InitializeServicesAndProgressState>();
	}
}