using Plugins.Joystick_Pack.Scripts.Base;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
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

		void Construct(IGameplayInterfacePresenter gameplayInterfacePresenter,
			int cashScore,
			int globalScore,
			int maxCashScore,
			int maxGlobalScore,
			int moneyCount,
			bool isHalfScoreReached,
			bool isScoresViewed);

		void FillSpeedButtonImage(float fillAmount);

		void SetActiveGoToNextLevelButton(bool isActive);
		void SetCashScore(int newScore);
		void SetMaxCashScore(int maxScore);
		void SetSoftCurrencyText(int newMoney);
		void SetTotalResourceScore(int newScore);
	}
}
