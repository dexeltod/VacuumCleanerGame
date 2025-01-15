using System;
using Sources.BuisenessLogic.Interfaces.Common.Factories;
using Sources.BuisenessLogic.Repository;
using Sources.BuisenessLogic.StateMachine.GameStates;
using Sources.BuisenessLogic.States;

namespace Sources.BuisenessLogic.Interfaces.Factory.StateMachine
{
	public class GameStatesRepositoryFactory : IFactory<GameStateMachineRepository>
	{
		private readonly MenuState _menuState;
		private readonly BuildSceneState _buildSceneState;
		private readonly GameLoopState _gameLoopState;

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

		public GameStateMachineRepository Create()
		{
			var gameStatesRepository = new GameStateMachineRepository();

			gameStatesRepository.Set(_menuState as IMenuState);
			gameStatesRepository.Set(_buildSceneState as IBuildSceneState);
			gameStatesRepository.Set(_gameLoopState);

			return gameStatesRepository;
		}
	}
}