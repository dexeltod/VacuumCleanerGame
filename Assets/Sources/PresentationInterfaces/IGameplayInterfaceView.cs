using Joystick_Pack.Scripts.Base;
using Sources.Controllers;
using Sources.PresentationInterfaces.Common;
using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.PresentationInterfaces
{
	public interface IGameplayInterfaceView : IPresentableView<IGameplayInterfacePresenter>
	{
		void Construct(
			IGameplayInterfacePresenter gameplayInterfacePresenter,
			int cashScore,
			int globalScore,
			int maxCashScore,
			int maxGlobalScore,
			int moneyCount,
			bool isHalfScoreReached
		);

		void SetActiveGoToNextLevelButton(bool isActive);
		void SetMaxCashScore(int newScore);
		void SetGlobalScore(int newScore);
		void SetCashScore(int newScore);
		void SetSoftCurrencyText(int newMoney);
		
		public Joystick Joystick { get; }
	}
}