using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sources.Application.StateMachine.GameStates;
using Sources.Application.StateMachineInterfaces;
using Sources.DIService;
using Sources.View.SceneEntity;

namespace Sources.Application.StateMachine
{
	public class GameStateMachine : IGameStateMachine
	{
		private readonly Dictionary<Type, IExitState> _states;

		private string _currentMusicName;
		private IExitState _activeState;

		public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain loadingCurtain,
			GameServices gameServices)
		{
			_states = new Dictionary<Type, IExitState>
			{
				[typeof(InitializeServicesAndProgressState)] =
					new InitializeServicesAndProgressState(this, gameServices, sceneLoader),

				[typeof(InitializeServicesWithProgressState)] =
					new InitializeServicesWithProgressState(this, gameServices),

				[typeof(MenuState)] = new MenuState(sceneLoader, loadingCurtain),

				[typeof(SceneLoadState)] = new SceneLoadState(this, sceneLoader, loadingCurtain, gameServices),

				[typeof(GameLoopState)] = new GameLoopState(this),
			};

		}

		public async UniTask Enter<TState>() where TState : class, IGameState
		{
			var state = ChangeState<TState>();
			await state.Enter();
		}

		public async UniTask Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>
		{
			TState state = ChangeState<TState>();
			await state.Enter(payload);
		}

		public async UniTask Enter<TState, TPayload, T>(TPayload payload, string musicName,
			bool isLevelNameIsStopMusicBetweenScenes
		) where TState : class, IPayloadState<TPayload>
		{
			TState state = ChangeState<TState>();
			await state.Enter(payload);

			SetOrStopMusic<TState, TPayload>(isLevelNameIsStopMusicBetweenScenes, musicName);
		}

		private void SetOrStopMusic<TState, TPayload>(bool isMusicStopped, string musicName)
			where TState : class, IPayloadState<TPayload>
		{
			if (musicName == _currentMusicName || string.IsNullOrWhiteSpace(musicName) == true)
				return;
			
			//TODO: Need to create music... maybe.
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