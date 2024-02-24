using System.Collections.Generic;
using Sources.ControllersInterfaces;
using Sources.PresentationInterfaces.Common;
using UnityEngine;

namespace Sources.PresentationInterfaces
{
	public interface IUpgradeWindow : IPresentableView<IUpgradeWindowPresenter>
	{
		void SetActiveYesNoButtons(bool isActive);
		Transform ContainerTransform { get; }
		List<string> Phrases { get; set; }

		void Construct(IUpgradeWindowPresenter presenter, int money);
		void SetMoney(int money);
	}
}