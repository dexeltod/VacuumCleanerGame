using Sources.Infrastructure.UI;
using Sources.InfrastructureInterfaces;
using Sources.Services;
using UnityEngine.UIElements;

namespace Sources.Application.MainMenu
{
	public abstract class MenuElement : IVisualElement
	{
		public VisualElement ThisElement { get; }

		protected readonly VisualElementSwitcher VisualElementController;
		protected readonly UIElementGetterFacade ElementGetter;

		protected MenuElement(VisualElement thisElement, VisualElementSwitcher visualElementSwitcher,
			UIElementGetterFacade elementGetter)
		{
			ThisElement = thisElement;
			VisualElementController = visualElementSwitcher;
			ElementGetter = elementGetter;
		}
	}
}