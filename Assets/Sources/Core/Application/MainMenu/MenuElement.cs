using Sources.Infrastructure.InfrastructureInterfaces;
using Sources.Infrastructure.Services;
using Sources.Infrastructure.UI;
using UnityEngine.UIElements;

namespace Sources.Core.Application.MainMenu
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