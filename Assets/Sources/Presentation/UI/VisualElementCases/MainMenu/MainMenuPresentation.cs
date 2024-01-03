using Sources.Utils;
using UnityEngine.UIElements;

namespace Sources.Presentation.UI.VisualElementCases.MainMenu
{
	public class MainMenuPresentation
	{
		private const string MainMenuName = "MainMenu";
		private readonly VisualElementGetter _visualElementGetter;

		private VisualElement _mainMenu;

		public MainMenuPresentation(VisualElementGetter visualElementGetter) =>
			_visualElementGetter = visualElementGetter;

		public void Initialize() =>
			_mainMenu = _visualElementGetter.GetFirst<VisualElement>(MainMenuName);
	}
}