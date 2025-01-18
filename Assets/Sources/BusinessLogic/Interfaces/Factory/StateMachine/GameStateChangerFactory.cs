using System;
using Sources.BusinessLogic.Interfaces.Common.Factories;
using Sources.BusinessLogic.Services;
using Sources.BusinessLogic.StateMachine.Common;

namespace Sources.BusinessLogic.Interfaces.Factory.StateMachine
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
