using System;
using Sources.Infrastructure.Common.Factory;
using Sources.Infrastructure.Services;
using Sources.Infrastructure.StateMachine.Common;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Services;
using VContainer;

namespace Sources.Infrastructure.Factories.StateMachine
{
	public class GameStateChangerFactory : Factory<IGameStateChanger>, IGameStateChangerFactory
	{
		private readonly GameStatesRepositoryFactory _gameStatesRepositoryFactory;

		[Inject]
		public GameStateChangerFactory(GameStatesRepositoryFactory gameStatesRepositoryFactory) =>
			_gameStatesRepositoryFactory = gameStatesRepositoryFactory ??
				throw new ArgumentNullException(nameof(gameStatesRepositoryFactory));

		public override IGameStateChanger Create() =>
			new GameStateChanger(new GameStateContainer(), _gameStatesRepositoryFactory.Create());
	}
}