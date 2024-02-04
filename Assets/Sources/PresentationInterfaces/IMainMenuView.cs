using System;

namespace Sources.PresentationInterfaces
{
	public interface IMainMenuView
	{
		event Action PlayButtonPressed;
		event Action DeleteSavesButtonPressed;
	}
}