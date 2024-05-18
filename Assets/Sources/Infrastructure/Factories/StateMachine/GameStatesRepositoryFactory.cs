using System;
using Sources.Infrastructure.Common.Factory;
using Sources.Infrastructure.Repository;
using Sources.Infrastructure.StateMachine.GameStates;
using Sources.InfrastructureInterfaces.Configs;
using Sources.InfrastructureInterfaces.States;
using Sources.InfrastructureInterfaces.States.StateMachineInterfaces;
using UnityEngine;
using VContainer;

namespace Sources.Infrastructure.Factories.StateMachine
{
	public class GameStatesRepositoryFactory : Factory<GameStateMachineRepository>
	{
		private readonly MenuState _menuState;
		private readonly BuildSceneState _buildSceneState;
		private readonly GameLoopState _gameLoopState;
		private readonly IObjectResolver _resolver;

		[Inject]
		public GameStatesRepositoryFactory(
			MenuState menuState,
			BuildSceneState buildSceneState,
			GameLoopState gameLoopState
		)
		{
			_menuState = menuState ?? throw new ArgumentNullException(nameof(menuState));
			_buildSceneState = buildSceneState ?? throw new ArgumentNullException(nameof(buildSceneState));
			_gameLoopState = gameLoopState ?? throw new ArgumentNullException(nameof(gameLoopState));
		}

		public override GameStateMachineRepository Create()
		{
			var gameStatesRepository = new GameStateMachineRepository();
			Debug.Log("gameStatesRepository.Set(_resolver.Resolve<MenuState>())");
			gameStatesRepository.Set(_menuState as IMenuState);

			Debug.Log("gameStatesRepository.Set(_resolver.Resolve<BuildSceneState>())");
			gameStatesRepository.Set(_buildSceneState as IBuildSceneState);

			Debug.Log("gameStatesRepository.Set(_resolver.Resolve<GameLoopState>())");
			gameStatesRepository.Set(_gameLoopState as IGameState);

			return gameStatesRepository;
		}
	}
}