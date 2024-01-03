using System;
using System.Collections.Generic;
using Sources.Application.StateMachine;
using Sources.Application.StateMachine.GameStates;
using Sources.ApplicationServicesInterfaces.StateMachineInterfaces;
using Sources.DIService;
using Sources.Presentation.SceneEntity;
using Sources.ServicesInterfaces;
using Sources.UseCases.Scene;

namespace Sources.Application
{
	public class GameStateMachineFactory
	{
		private readonly ISceneLoader _sceneLoader;
		private readonly IAssetProvider _assetProvider;
		private readonly ICoroutineRunner _coroutineRunner;
		private readonly LoadingCurtain _loadingCurtain;

		public GameStateMachineFactory(
			ISceneLoader sceneLoader,
			IAssetProvider assetProvider,
			LoadingCurtain loadingCurtain,
			ICoroutineRunner coroutineRunner
		)
		{
			_sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
			_assetProvider = assetProvider ?? throw new ArgumentNullException(nameof(assetProvider));
			_coroutineRunner = coroutineRunner ?? throw new ArgumentNullException(nameof(coroutineRunner));
			_loadingCurtain = loadingCurtain ? loadingCurtain : throw new ArgumentNullException(nameof(loadingCurtain));
		}

		public GameStateMachine Create(
		)
		{
			GameStateMachine gameStateMachine = new GameStateMachine();

			gameStateMachine.Initialize
			(
				new Dictionary<Type, IExitState>()
				{
					[typeof(InitializeServicesAndProgressState)] =
						new InitializeServicesAndProgressState(
							gameStateMachine,
							ServiceLocator.Container,
							_sceneLoader,
							_assetProvider
						),

					[typeof(InitializeServicesWithProgressState)] =
						new InitializeServicesWithProgressState(
							gameStateMachine,
							ServiceLocator.Container,
							_loadingCurtain
						),

					[typeof(MenuState)] = new MenuState(_sceneLoader, _loadingCurtain, ServiceLocator.Container),

					[typeof(BuildSandState)] = new BuildSandState(
						gameStateMachine,
						_sceneLoader,
						ServiceLocator.Container,
						_loadingCurtain
					),

					[typeof(InitializeServicesWithViewState)]
						= new InitializeServicesWithViewState(gameStateMachine, ServiceLocator.Container),

					[typeof(BuildSceneState)] = new BuildSceneState(gameStateMachine, ServiceLocator.Container, _coroutineRunner),

					[typeof(GameLoopState)] = new GameLoopState(gameStateMachine, _loadingCurtain)
				}
			);

			return gameStateMachine;
		}
	}
}