using Sources.InfrastructureInterfaces.Configs;
using Sources.InfrastructureInterfaces.States.StateMachineInterfaces;

namespace Sources.InfrastructureInterfaces.States
{
	public interface IBuildSceneState : IGameState<ILevelConfig> { }
}