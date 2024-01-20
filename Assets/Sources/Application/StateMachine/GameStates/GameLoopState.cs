using System;
using Sources.ApplicationServicesInterfaces.StateMachineInterfaces;
using Sources.Presentation.SceneEntity;
using Sources.PresentationInterfaces;
using VContainer;

namespace Sources.Application.StateMachine.GameStates
{
	public sealed class GameLoopState : IGameState
	{
		private readonly GameStateMachine _gameStateMachine;
		private readonly LoadingCurtain _loadingCurtain;
		private IGameplayInterfaceView _gameplayInterface;

		[Inject]
		public GameLoopState(
			GameStateMachine gameStateMachine,
			LoadingCurtain loadingCurtain,
			IGameplayInterfaceView gameplayInterface
		)
		{
			_gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
			_gameplayInterface = gameplayInterface;
			_loadingCurtain = loadingCurtain ? loadingCurtain : throw new ArgumentNullException(nameof(loadingCurtain));
		}

		public void Enter()
		{
			_gameplayInterface.GameObject.SetActive(true);

			_loadingCurtain.HideSlowly();
		}

		public void Exit()
		{
			if (_gameplayInterface != null)
			{
				_gameplayInterface.Destroying -= OnDestroying;
				_gameplayInterface.GameObject.SetActive(false);
			}

			_loadingCurtain.Show();
		}

		private void OnDestroying() =>
			_gameplayInterface = null;
	}
}