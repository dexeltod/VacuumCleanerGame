using System;
using Sources.Boot.Scripts.States.StateMachine.Common;
using Sources.BusinessLogic;
using Sources.BusinessLogic.Interfaces.Common.Factories;
using Sources.BusinessLogic.Interfaces.Factory;
using Sources.BusinessLogic.Services;

namespace Sources.Boot.Scripts.Factories.StateMachine
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