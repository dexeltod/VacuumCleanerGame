using System;
using Sources.Infrastructure.Common.Factory;
using Sources.Infrastructure.Repository;
using Sources.Infrastructure.StateMachine.GameStates;
using Sources.InfrastructureInterfaces.States;
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
			gameStatesRepository.Set(_menuState as IMenuState);

			gameStatesRepository.Set(_buildSceneState as IBuildSceneState);

			gameStatesRepository.Set(_gameLoopState);

			return gameStatesRepository;
		}
	}
}