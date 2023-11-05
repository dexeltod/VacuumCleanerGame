using System;
using System.ComponentModel.Design;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using UnityEngine;

namespace Sources.ServicesInterfaces.UI
{
	public interface IUpgradeWindow
	{
		event Action<bool> ActiveChanged;
		event Action       Destroyed;
		void               OnEnable();
		void               OnDisable();
		void               OnDestroy();
		void               SetActiveYesNoButtons(bool isActive);
		Transform          ContainerTransform {get;}
		void               Construct(IResourceReadOnly<int> resource);
	}
}