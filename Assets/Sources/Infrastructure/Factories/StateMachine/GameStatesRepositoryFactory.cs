using System;
using Sources.Infrastructure.Common.Factory;
using Sources.Infrastructure.Repository;
using Sources.Infrastructure.StateMachine.GameStates;
using Sources.InfrastructureInterfaces.States;
using UnityEngine;
using VContainer;

namespace Sources.Infrastructure.Factories.StateMachine
{
	public class GameStatesRepositoryFactory : Factory<GameStateMachineRepository>
	{
		private readonly IMenuState _menuState;
		private readonly IBuildSceneState _buildSceneState;
		private readonly IGameLoopState _gameLoopState;

		[Inject]
		public GameStatesRepositoryFactory(
			IMenuState menuState,
			IBuildSceneState buildSceneState,
			IGameLoopState gameLoopState
		)
		{
			_menuState = menuState ?? throw new ArgumentNullException(nameof(menuState));
			_buildSceneState = buildSceneState ?? throw new ArgumentNullException(nameof(buildSceneState));
			_gameLoopState = gameLoopState ?? throw new ArgumentNullException(nameof(gameLoopState));
		}

		public override GameStateMachineRepository Create()
		{
			GameStateMachineRepository gameStatesRepository = new GameStateMachineRepository();

			Debug.Log("ща тут будет резолвиться ");

			Debug.Log("зарезолвили первую ситуацию( мы её не зарезолвили)");
			gameStatesRepository.Set(_menuState);

			Debug.Log("ща тут будет резолвиться  2");
			gameStatesRepository.Set(_buildSceneState);

			Debug.Log("ща тут будет резолвиться  3");
			gameStatesRepository.Set(_gameLoopState);

			return gameStatesRepository;
		}
	}
}