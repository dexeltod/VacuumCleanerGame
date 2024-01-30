using System;
using Joystick_Pack.Scripts.Base;
using Sources.ServicesInterfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.PresentationInterfaces
{
	public interface IGameplayInterfaceView : IGoToTextLevelButtonSubscribable
	{
		public GameObject GameObject { get; }
		public Canvas Canvas { get; }
		public Image ScoreSlider { get; }
		public TextMeshProUGUI ScoreText { get; }
		public TextMeshProUGUI MoneyText { get; }
		public Joystick Joystick { get; }
		Button GoToNextLevelButton { get; }

		event Action Destroying;

		void Construct(
			int startCashScore,
			int maxCashScore,
			int maxGlobalScore,
			int moneyCount,
			IResourceProgressEventHandler resourceProgressEventHandler,
			bool isActiveOnStart
		);
	}
}