using Sources.Application.StateMachine;
using Sources.Application.StateMachine.GameStates;
using Sources.DIService;
using Sources.PresentationInterfaces;
using Sources.View.SceneEntity;

namespace Sources.Application
{
	public class Game
	{
		private readonly GameStateMachine _stateMachine;

		public Game
		(
			LoadingCurtain              loadingCurtain,
			IYandexAuthorizationHandler yandexAuthorizationHandler
		) =>
			_stateMachine = new GameStateMachine
			(
				new SceneLoader(),
				loadingCurtain,
				yandexAuthorizationHandler,
				GameServices.Container
			);

		public void Start() =>
			_stateMachine.Enter<InitializeServicesAndProgressState>();
	}
}