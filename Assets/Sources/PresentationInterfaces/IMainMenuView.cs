using System;
using Sources.ControllersInterfaces;
using Sources.PresentationInterfaces.Common;

namespace Sources.PresentationInterfaces
{
	public interface IMainMenuView : IPresentableView<IMainMenuPresenter>
	{
		event Action PlayButtonPressed;
		event Action DeleteSavesButtonPressed;
	}
}