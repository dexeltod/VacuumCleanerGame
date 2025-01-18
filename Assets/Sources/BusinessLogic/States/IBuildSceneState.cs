using Sources.BusinessLogic.States.StateMachineInterfaces;
using Sources.DomainInterfaces;

namespace Sources.BusinessLogic.States
{
	public interface IBuildSceneState : IGameState<ILevelConfig>
	{
	}
}
