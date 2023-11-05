using System;
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
		public Image           ScoreSlider { get; }
		public TextMeshProUGUI ScoreText   { get; }
		public TextMeshProUGUI MoneyText   { get; }
		public Joystick        Joystick    { get; }

		event Action Destroying;

		void Construct
		(
			int                           startCashScore,
			int                           maxScore,
			int                           moneyCount,
			IResourceProgressEventHandler resourcesProgressPresenter,
			bool                          isActiveOnStart
		);
	}
}