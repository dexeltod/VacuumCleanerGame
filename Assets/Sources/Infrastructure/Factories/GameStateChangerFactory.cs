using System;
using Sources.Infrastructure.Common.Factory;
using Sources.Infrastructure.Services;
using Sources.Infrastructure.StateMachine.Common;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Repository;
using Sources.InfrastructureInterfaces.Services;
using VContainer;

namespace Sources.Infrastructure.Factories
{
	public class GameStateChangerFactory : Factory<IGameStateChanger>, IGameStateChangerFactory
	{
		private readonly GameStateContainerFactory _containerFactory;
		private readonly GameStatesRepositoryFactory _gameStatesRepositoryFactory;

		[Inject]
		public GameStateChangerFactory(
			GameStateContainerFactory containerFactor,
			GameStatesRepositoryFactory gameStatesRepositoryFactory
		)
		{
			_containerFactory = containerFactor ?? throw new ArgumentNullException(nameof(containerFactor));
			_gameStatesRepositoryFactory = gameStatesRepositoryFactory ??
				throw new ArgumentNullException(nameof(gameStatesRepositoryFactory));
		}

		public override IGameStateChanger Create()
		{
			IGameStateMachineRepository gameStatesRepository = _gameStatesRepositoryFactory.Create();

			GameStateContainer container = _containerFactory.Create();

			GameStateChanger gameStateChanger = new GameStateChanger(container, gameStatesRepository);

			return gameStateChanger;
		}
	}
}