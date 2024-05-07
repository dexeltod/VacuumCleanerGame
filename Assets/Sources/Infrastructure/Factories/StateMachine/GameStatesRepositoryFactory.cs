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
		private readonly IObjectResolver _resolver;

		[Inject]
		public GameStatesRepositoryFactory(IObjectResolver resolver) =>
			_resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));

		public override GameStateMachineRepository Create()
		{
			var gameStatesRepository = new GameStateMachineRepository();

			gameStatesRepository.Set(_resolver.Resolve<IMenuState>());
			gameStatesRepository.Set(_resolver.Resolve<IBuildSceneState>());
			gameStatesRepository.Set(_resolver.Resolve<GameLoopState>());

			return gameStatesRepository;
		}
	}
}