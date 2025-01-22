using Plugins.Joystick_Pack.Scripts.Base;

namespace Sources.ControllersInterfaces
{
	public interface IGameplayInterfacePresenter : IPresenter
	{
		public Joystick Joystick { get; }

		public void OnGoToNextLevel();
		public void OnIncreaseSpeed();
		public void SetActiveGoToNextLevelButton(bool isActive);
		public void SetCashScore(int cashScore);
		public void SetSoftCurrency(int soft);
		public void SetTotalResourceCount(int globalScore);
	}
}