using System;
using Sources.BuisenessLogic.Interfaces.Common.Factories;
using Sources.BuisenessLogic.Services;
using Sources.BuisenessLogic.StateMachine.Common;

namespace Sources.BuisenessLogic.Interfaces.Factory.StateMachine
{
	public class GameStateChangerFactory : IFactory<IGameStateChanger>, IGameStateChangerFactory
	{
		private readonly GameStatesRepositoryFactory _gameStatesRepositoryFactory;

		public GameStateChangerFactory(GameStatesRepositoryFactory gameStatesRepositoryFactory)
		{
			_gameStatesRepositoryFactory = gameStatesRepositoryFactory ??
			                               throw new ArgumentNullException(nameof(gameStatesRepositoryFactory));
		}

		public IGameStateChanger Create() =>
			new GameStateChanger(new GameStateContainer(), _gameStatesRepositoryFactory.Create());
	}
}
