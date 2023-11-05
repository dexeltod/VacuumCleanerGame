using System;
using System.Collections.Generic;
using Sources.Application.StateMachine;
using Sources.Application.StateMachine.GameStates;
using Sources.DIService;
using Sources.View.SceneEntity;
using UnityEngine;

namespace Sources.Application
{
	public class Bootstrapper : MonoBehaviour, ICoroutineRunner
	{
		[SerializeField] private LoadingCurtain _loadingCurtain;
		[SerializeField] private YandexAuthorizationHandler _authorizationHandler;

		private Game _game;

		private void Awake()
		{
			DontDestroyOnLoad(this);

			ServiceLocator.Initialize();

			_loadingCurtain.gameObject.SetActive(true);

			SceneLoader sceneLoader = new SceneLoader();

			GameStateMachine gameStateMachine = CreateGameStateMachine(sceneLoader);

			_game = new Game(gameStateMachine);

			StartGame();
		}

		private GameStateMachine CreateGameStateMachine(SceneLoader sceneLoader)
		{
			GameStateMachine gameStateMachine = new GameStateMachine();

			gameStateMachine.Initialize
			(
				new Dictionary<Type, IExitState>()
				{
					[typeof(InitializeServicesAndProgressState)] =
						new InitializeServicesAndProgressState
						(
							_authorizationHandler,
							gameStateMachine,
							ServiceLocator.Container,
							sceneLoader
						),

					[typeof(InitializeServicesWithProgressState)] =
						new InitializeServicesWithProgressState
						(
							gameStateMachine,
							ServiceLocator.Container,
							_loadingCurtain
						),

					[typeof(MenuState)] = new MenuState(sceneLoader, _loadingCurtain, ServiceLocator.Container),

					[typeof(BuildSandState)] = new BuildSandState
					(
						gameStateMachine,
						sceneLoader,
						ServiceLocator.Container,
						_loadingCurtain
					),

					[typeof(InitializeServicesWithViewState)] = new InitializeServicesWithViewState
						(gameStateMachine, ServiceLocator.Container),

					[typeof(BuildSceneState)] = new BuildSceneState
						(gameStateMachine, ServiceLocator.Container),

					[typeof(GameLoopState)] = new GameLoopState(gameStateMachine, _loadingCurtain)
				}
			);

			return gameStateMachine;
		}

		private void StartGame() =>
			_game.Start();

		public void StopCoroutineRunning(Coroutine coroutine) =>
			StopCoroutine(coroutine);
	}
}