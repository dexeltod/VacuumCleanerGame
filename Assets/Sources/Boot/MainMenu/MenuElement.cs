using Sources.BusinessLogic.UiToolkitElements;
using Sources.Infrastructure.Services;
using Sources.Infrastructure.UI;
using UnityEngine.UIElements;

namespace Sources.Boot.MainMenu
{
	public abstract class MenuElement : IVisualElement
	{
		protected readonly UIElementGetterFacade ElementGetter;

		protected readonly VisualElementSwitcher VisualElementController;

		protected MenuElement(VisualElement thisElement,
			VisualElementSwitcher visualElementSwitcher,
			UIElementGetterFacade elementGetter)
		{
			ThisElement = thisElement;
			VisualElementController = visualElementSwitcher;
			ElementGetter = elementGetter;
		}

		public VisualElement ThisElement { get; }
	}
}