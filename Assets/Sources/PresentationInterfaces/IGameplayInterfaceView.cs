using Joystick_Pack.Scripts.Base;
using Sources.ServicesInterfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.PresentationInterfaces
{
	public interface IGameplayInterfaceView : IGoToTextLevelButtonSubscribeable
	{
		public GameObject      GameObject  { get; }
		public Canvas          Canvas      { get; }
		public Slider          ScoreSlider { get; }
		public TextMeshProUGUI ScoreText   { get; }
		public TextMeshProUGUI MoneyText   { get; }
		public Joystick        Joystick    { get; }
		void                   SetGlobalScore(int newScore);

		void Construct
		(
			int maxScore,
			int moneyCount
		);

		void SetMoney(int                      newMoney);
		void SetScore(int                      newScore);
		void SetCurrentLevel(int               newLevel);
		void SetActiveGoToNextLevelButton(bool isActive);
	}
}