using Sources.Infrastructure.Common.Factory;
using Sources.Infrastructure.StateMachine.Common;

namespace Sources.Infrastructure.Factories
{
	public class GameStateContainerFactory : Factory<GameStateContainer>
	{
		public override GameStateContainer Create() =>
			new GameStateContainer();
	}
}