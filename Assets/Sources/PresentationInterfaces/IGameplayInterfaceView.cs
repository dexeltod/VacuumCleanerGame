using System;
using Joystick_Pack.Scripts.Base;
using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.PresentationInterfaces
{
	public interface IGameplayInterfaceView : IGoToTextLevelButtonObserver
	{
		public GameObject GameObject { get; }
		public Canvas Canvas { get; }
		public Joystick Joystick { get; }
		ITmpPhrases Phrases { get; }

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