using Joystick_Pack.Scripts.Base;
using Sources.ControllersInterfaces;
using Sources.PresentationInterfaces.Common;
using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.PresentationInterfaces
{
	public interface IGameplayInterfaceView : IPresentableView<IGameplayInterfacePresenter>
	{
		public GameObject GameObject { get; }
		public Canvas Canvas { get; }
		public Joystick Joystick { get; }
		ITmpPhrases Phrases { get; }

		void Construct(
			IGameplayInterfacePresenter gameplayInterfacePresenter,
			int startCashScore,
			int maxCashScore,
			int maxGlobalScore,
			int moneyCount,
			IResourceProgressEventHandler resourceProgressEventHandler,
			bool isActiveOnStart
		);

		void SetActiveGoToNextLevelButton(bool isActive);
		void SetMaxCashScore(int newScore);
		void SetGlobalScore(int newScore);
		void SetCashScore(int newScore);
		void SetSoftCurrency(int newMoney);
	}
}