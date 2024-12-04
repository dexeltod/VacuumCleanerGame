using Graphic.Joystick_Pack.Scripts.Base;
using Sources.ControllersInterfaces.Common;

namespace Sources.ControllersInterfaces
{
	public interface IGameplayInterfacePresenter : IPresenter
	{
		public Joystick Joystick { get; }

		public void OnGoToNextLevel();
		public void OnIncreaseSpeed();
		public void SetActiveGoToNextLevelButton(bool isActive);
		public void SetTotalResourceCount(int globalScore);
		public void SetCashScore(int cashScore);
		public void SetSoftCurrency(int soft);
	}
}