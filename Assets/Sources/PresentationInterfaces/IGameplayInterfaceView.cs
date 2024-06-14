using Graphic.Joystick_Pack.Scripts.Base;
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

		void Construct(
			IGameplayInterfacePresenter gameplayInterfacePresenter,
			int cashScore,
			int globalScore,
			int maxCashScore,
			int maxGlobalScore,
			int moneyCount,
			bool isHalfScoreReached,
			bool isScoresViewed
		);

		void SetActiveGoToNextLevelButton(bool isActive);
		void SetMaxCashScore(int maxScore);
		void SetTotalResourceScore(int newScore);
		void SetCashScore(int newScore);
		void SetSoftCurrencyText(int newMoney);
		void FillSpeedButtonImage(float fillAmount);
	}
}