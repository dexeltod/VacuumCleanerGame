using Plugins.Joystick_Pack.Scripts.Base;
using Sources.ControllersInterfaces;
using Sources.PresentationInterfaces.Common;
using UnityEngine.UI;

namespace Sources.PresentationInterfaces
{
	public interface IGameplayInterfaceView : IPresentableView<IGameplayInterfacePresenter>
	{
		public Joystick Joystick { get; }
		Image IncreaseSpeedButtonImage { get; }
		Button GoToNextLevelButton { get; }
		Button IncreaseSpeedButton { get; }

		void FillSpeedButtonImage(float fillAmount);

		void SetActiveGoToNextLevelButton(bool isActive);
		void SetCashScore(int newScore);
		void SetMaxCashScore(int maxScore);
		void SetSoftCurrencyText(int newMoney);
		void SetTotalResourceScore(int newScore);
	}
}