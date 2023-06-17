using System;
using System.Collections.Generic;
using Model;
using Model.DI;
using View.UI;
using ViewModel.Infrastructure.Services;
using ViewModel.Infrastructure.StateMachine.GameStates;

namespace ViewModel.Infrastructure.StateMachine
{
	public class GameStateMachine : IGameStateMachine
	{
		private readonly IMusicService _musicService;
		private readonly Dictionary<Type, IExitState> _states;

		private string _currentMusicName;
		private IExitState _activeState;

		public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain loadingCurtain,
			ServiceLocator serviceLocator, MusicSetter musicSetter)
		{
			_states = new Dictionary<Type, IExitState>
			{
				[typeof(InitializeServicesState)] =
					new InitializeServicesState(this, serviceLocator, musicSetter, sceneLoader),

				[typeof(LoadProgressState)] = new LoadProgressState(this),

				[typeof(InitializeServicesWithProgressState)] =
					new InitializeServicesWithProgressState(this, serviceLocator),

				[typeof(MenuState)] = new MenuState(sceneLoader, loadingCurtain),

				[typeof(SceneLoadState)] = new SceneLoadState(this, sceneLoader, loadingCurtain, serviceLocator),

				[typeof(GameLoopState)] = new GameLoopState(this),
			};

			_musicService = serviceLocator.GetSingle<IMusicService>();
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

			if (isLevelNameIsStopMusicBetweenScenes)
				_musicService.Stop();

			_musicService.Set(musicName);
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