using Sources.BuisenessLogic.Services;

namespace Sources.BuisenessLogic.Interfaces.Factory
{
	public interface IGameStateChangerFactory
	{
		IGameStateChanger Create();
	}
}