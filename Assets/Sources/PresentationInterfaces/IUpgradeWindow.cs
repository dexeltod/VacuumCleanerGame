using System;
using System.Collections.Generic;
using Sources.PresentationInterfaces.Common;
using Sources.PresentersInterfaces;
using UnityEngine;

namespace Sources.PresentationInterfaces
{
	public interface IUpgradeWindow : IPresentableView<IUpgradeWindowPresenter>
	{
		event Action<bool> ActiveChanged;
		event Action Destroyed;
		void Enable();
		void Disable();
		void OnDestroy();
		void SetActiveYesNoButtons(bool isActive);
		Transform ContainerTransform { get; }
		List<string> Phrases { get; set; }
	}
}