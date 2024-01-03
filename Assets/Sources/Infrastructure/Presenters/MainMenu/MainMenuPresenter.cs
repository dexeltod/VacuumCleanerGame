using Sources.Presentation.UI.VisualElementCases.MainMenu;

namespace Sources.Infrastructure.Presenters.MainMenu
{
	public class MainMenuPresenter
	{
		private readonly MainMenuPresentation _mainMenuPresentation;

		public MainMenuPresenter(MainMenuPresentation mainMenuPresentation)
		{
			_mainMenuPresentation = mainMenuPresentation;
		}
	}
}