using Sources.InfrastructureInterfaces.Services;

namespace Sources.InfrastructureInterfaces.Factory
{
	public interface IGameStateChangerFactory
	{
		IGameStateChanger Create();
	}
}