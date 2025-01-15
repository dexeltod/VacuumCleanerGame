using Sources.BuisenessLogic.UiToolkitElements;
using Sources.Infrastructure.Services;
using Sources.Infrastructure.UI;
using UnityEngine.UIElements;

namespace Sources.Boot.MainMenu
{
	public abstract class MenuElement : IVisualElement
	{
		public VisualElement ThisElement { get; }

		protected readonly VisualElementSwitcher VisualElementController;
		protected readonly UIElementGetterFacade ElementGetter;

		protected MenuElement(VisualElement thisElement,
			VisualElementSwitcher visualElementSwitcher,
			UIElementGetterFacade elementGetter)
		{
			ThisElement = thisElement;
			VisualElementController = visualElementSwitcher;
			ElementGetter = elementGetter;
		}
	}
}