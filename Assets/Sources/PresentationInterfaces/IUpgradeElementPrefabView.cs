using Sources.ControllersInterfaces;
using Sources.PresentationInterfaces.Common;
using UnityEngine;

namespace Sources.PresentationInterfaces
{
	public interface IUpgradeElementPrefabView : IPresentableView<IUpgradeElementPresenter>,
		IUpgradeElementChangeableView
	{
		void Construct(
			IUpgradeElementPresenter presenter,
			int id,
			Sprite icon,
			int boughtPointsCount,
			int price,
			string title,
			string description,
			int maxPoints
		);
	}
}