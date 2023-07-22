using System;
using UnityEngine;

namespace Sources.View.Interfaces
{
	public interface IUpgradeWindow
	{
		event Action<bool> ActiveChanged;
		event Action Destroyed;
		void OnEnable();
		void OnDisable();
		void OnDestroy();
		void SetActiveYesNoButtons(bool isActive);
		Transform UpgradeElementsTransform {get;}
		void Construct();
	}
}