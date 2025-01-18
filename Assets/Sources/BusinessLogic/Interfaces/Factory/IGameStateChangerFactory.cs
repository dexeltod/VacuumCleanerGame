using Sources.BusinessLogic.Services;

namespace Sources.BusinessLogic.Interfaces.Factory
{
	public interface IGameStateChangerFactory
	{
		IGameStateChanger Create();
	}
}
