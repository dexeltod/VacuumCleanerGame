using System;
using Sources.Boot.Scripts.States.StateMachine.GameStates;
using Sources.BusinessLogic.Repository;
using Sources.BusinessLogic.States;
using VContainer;

namespace Sources.Boot.Scripts.Factories.StateMachine
{
	public class GameStatesRepositoryInitializer
	{
		private readonly IObjectResolver _resolver;

		public GameStatesRepositoryInitializer(IObjectResolver resolver) =>
			_resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));

		public void Initialize(IGameStateMachineRepository gameStateMachineRepository)

		{
			var menuState = _resolver.Resolve<IMenuState>();
			var buildSceneState = _resolver.Resolve<IBuildSceneState>();
			var gameLoopState = _resolver.Resolve<GameLoopState>();

			// IExitableState[] exitableStates =
			// {
			// 	_resolver.Resolve<IMenuState>(),
			// 	_resolver.Resolve<IBuildSceneState>(),
			// 	_resolver.Resolve<GameLoopState>()
			// };

			gameStateMachineRepository.Set(menuState);
			gameStateMachineRepository.Set(buildSceneState);
			gameStateMachineRepository.Set(gameLoopState);

			// foreach (IExitableState state in exitableStates) gameStateMachinekRepository.Set(state);
		}
	}
}