using Sources.ControllersInterfaces;
using Sources.PresentationInterfaces.Common;
using UnityEngine;

namespace Sources.PresentationInterfaces
{
	public interface IUpgradeElementPrefabView : IPresentableView<IUpgradeElementPresenter>,
		IUpgradeElementChangeableView
	{
		void Construct(
			Sprite icon,
			IUpgradeElementPresenter presenter,
			string title,
			string description,
			int id,
			int boughtPointsCount,
			int price,
			int maxPoints
		);
	}
}