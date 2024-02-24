using Sources.ControllersInterfaces.Common;

namespace Sources.ControllersInterfaces
{
	public interface IGameplayInterfacePresenter : IPresenter
	{
		void GoToNextLevel();
		void IncreaseSpeed();
	}
}