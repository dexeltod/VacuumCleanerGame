using System.Collections.Generic;
using Sources.ControllersInterfaces;
using Sources.PresentationInterfaces.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.PresentationInterfaces
{
	public interface IUpgradeWindowPresentation : IPresentableView<IUpgradeWindowPresenter>
	{
		Transform ContainerTransform { get; }
		List<string> Phrases { get; set; }
		Button CloseMenuButton { get; }
		GameObject UpgradeWindowMain { get; }
		AudioSource AudioClose { get; }

		void Construct(IUpgradeWindowPresenter presenter, int money);

		void SetMoney(int money);
	}
}
