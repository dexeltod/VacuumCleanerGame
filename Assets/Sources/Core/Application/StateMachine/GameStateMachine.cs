using System;
using System.Collections.Generic;
using Sources.Core.Application.StateMachine.GameStates;
using Sources.Core.Application.StateMachineInterfaces;
using Sources.Core.DI;

namespace Sources.Core.Application.StateMachine
{
	public class GameStateMachine : IGameStateMachine
	{
		private readonly Dictionary<Type, IExitState> _states;

		private string _currentMusicName;
		private IExitState _activeState;

		public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain loadingCurtain,
			ServiceLocator serviceLocator)
		{
			_states = new Dictionary<Type, IExitState>
			{
				[typeof(InitializeServicesAndProgressState)] =
					new InitializeServicesAndProgressState(this, serviceLocator, sceneLoader),

				[typeof(InitializeServicesWithProgressState)] =
					new InitializeServicesWithProgressState(this, serviceLocator),

				[typeof(MenuState)] = new MenuState(sceneLoader, loadingCurtain),

				[typeof(SceneLoadState)] = new SceneLoadState(this, sceneLoader, loadingCurtain, serviceLocator),

				[typeof(GameLoopState)] = new GameLoopState(this),
			};

		}

		public void Enter<TState>() where TState : class, IGameState
		{
			var state = ChangeState<TState>();
			state.Enter();
		}

		public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>
		{
			TState state = ChangeState<TState>();
			state.Enter(payload);
		}

		public void Enter<TState, TPayload, T>(TPayload payload, string musicName,
			bool isLevelNameIsStopMusicBetweenScenes
		) where TState : class, IPayloadState<TPayload>
		{
			TState state = ChangeState<TState>();
			state.Enter(payload);

			SetOrStopMusic<TState, TPayload>(isLevelNameIsStopMusicBetweenScenes, musicName);
		}

		private void SetOrStopMusic<TState, TPayload>(bool isLevelNameIsStopMusicBetweenScenes, string musicName)
			where TState : class, IPayloadState<TPayload>
		{
			if (musicName == _currentMusicName || string.IsNullOrWhiteSpace(musicName) == true)
				return;
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