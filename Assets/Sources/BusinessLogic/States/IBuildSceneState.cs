using Sources.BusinessLogic.States.StateMachineInterfaces;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces.Configs.Scripts.Level;

namespace Sources.BusinessLogic.States
{
	public interface IBuildSceneState : IGameState<LevelConfig>
	{
	}
}
