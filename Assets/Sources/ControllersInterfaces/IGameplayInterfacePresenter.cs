using Joystick_Pack.Scripts.Base;
using Sources.ControllersInterfaces.Common;

namespace Sources.ControllersInterfaces
{
	public interface IGameplayInterfacePresenter : IPresenter
	{
		public Joystick Joystick { get; }
		public void GoToNextLevel();
		public void IncreaseSpeed();
		public void SetActiveGoToNextLevelButton(bool isActive);
		public void SetGlobalScore(int globalScore);
		public void SetCashScore(int cashScore);
		public void SetSoftCurrency(int soft);
		
	}
}