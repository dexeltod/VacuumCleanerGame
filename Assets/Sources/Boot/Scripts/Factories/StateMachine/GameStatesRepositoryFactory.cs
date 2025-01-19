using System;
using System.Collections.Generic;
using Sources.Boot.Scripts.States.StateMachine.GameStates;
using Sources.BusinessLogic.Interfaces.Common.Factories;
using Sources.BusinessLogic.Repository;
using Sources.BusinessLogic.States;
using Sources.BusinessLogic.States.StateMachineInterfaces;
using VContainer;

namespace Sources.Boot.Scripts.Factories.StateMachine
{
	public class GameStatesRepositoryFactory : IFactory<GameStateMachineRepository>
	{
		private readonly IObjectResolver _resolver;

		public GameStatesRepositoryFactory(IObjectResolver resolver) =>
			_resolver = resolver;

		public GameStateMachineRepository Create()
		{
			var menuState = _resolver.Resolve<IMenuState>();
			var buildSceneState = _resolver.Resolve<BuildSceneState>();
			var gameLoopState = _resolver.Resolve<GameLoopState>();

			return new GameStateMachineRepository(
				new Dictionary<Type, IExitableState>()
				{
					{ typeof(IMenuState), menuState },
					{ typeof(IBuildSceneState), buildSceneState },
					{ typeof(GameLoopState), gameLoopState },
				}
			);
		}
	}
}
