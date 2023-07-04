using Infrastructure.Services;
using Infrastructure.UI;
using UnityEngine.UIElements;

namespace Application.MainMenu
{
	public abstract class MenuElement : IVisualElement
	{
		public VisualElement ThisElement { get; }
	
		protected readonly VisualElementViewModel VisualElementController;
		protected readonly UIElementGetterFacade ElementGetter;
	
		protected MenuElement(VisualElement thisElement, VisualElementViewModel visualElementSwitcher, UIElementGetterFacade elementGetter)
		{
			ThisElement = thisElement;
			VisualElementController = visualElementSwitcher;
			ElementGetter = elementGetter;
		}
	}
}