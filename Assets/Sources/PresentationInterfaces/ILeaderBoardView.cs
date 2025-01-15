using Sources.ControllersInterfaces;
using Sources.PresentationInterfaces.Common;
using UnityEngine;

namespace Sources.PresentationInterfaces
{
	public interface ILeaderBoardView : IPresentableView<IMainMenuPresenter>
	{
		Transform Container { get; }
		ILeaderBoardPlayerPanelBehaviour PlayerPanel { get; }
	}
}
