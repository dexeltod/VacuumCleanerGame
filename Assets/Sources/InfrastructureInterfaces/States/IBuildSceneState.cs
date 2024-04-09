using Sources.Infrastructure.Configs.Scripts.Level;
using Sources.InfrastructureInterfaces.States.StateMachineInterfaces;

namespace Sources.InfrastructureInterfaces.States
{
	public interface IBuildSceneState : IGameState<ILevelConfig> { }
}