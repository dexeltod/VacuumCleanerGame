using System;
using System.Collections.Generic;
using Sources.Application.StateMachine.GameStates;
using Sources.Application.StateMachineInterfaces;
using Sources.DIService;
using Sources.PresentationInterfaces;
using Sources.View.SceneEntity;

namespace Sources.Application.StateMachine
{
	public class GameStateMachine : IGameStateMachine
	{
		private readonly Dictionary<Type, IExitState> _states;

		private string     _currentMusicName;
		private IExitState _activeState;

		public GameStateMachine
		(
			SceneLoader                 sceneLoader,
			LoadingCurtain              loadingCurtain,
			IYandexAuthorizationHandler yandexAuthorizationHandler,
			GameServices                gameServices
		) =>
			_states = new Dictionary<Type, IExitState>
			{
				[typeof(InitializeServicesAndProgressState)] =
					new InitializeServicesAndProgressState
					(
						yandexAuthorizationHandler,
						this,
						gameServices,
						sceneLoader
					),

				[typeof(InitializeServicesWithProgressState)] =
					new InitializeServicesWithProgressState
					(
						this,
						gameServices,
						loadingCurtain
					),

				[typeof(MenuState)] = new MenuState(sceneLoader, loadingCurtain, gameServices),

				[typeof(BuildSceneState)] = new BuildSceneState
				(
					this,
					sceneLoader,
					loadingCurtain,
					gameServices
				),

				[typeof(GameLoopState)] = new GameLoopState(this)
			};

		public void Enter<TState>() where TState : class, IGameState
		{
			TState state = ChangeState<TState>();
			state.Enter();
		}

		public void Enter<TState, TPayload>(TPayload payload)
			where TState : class, IPayloadState<TPayload>
		{
			TState state = ChangeState<TState>();
			state.Enter(payload);
		}

		private TState ChangeState<TState>() where TState : class, IExitState
		{
			_activeState?.Exit();

			TState state = GetState<TState>();
			_activeState = state;

			return state;
		}

		private TState GetState<TState>() where TState : class, IExitState =>
			_states[typeof(TState)] as TState;
	}
}