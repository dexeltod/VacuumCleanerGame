using UnityEngine.UIElements;

namespace Sources.Infrastructure.UI
{
	public class VisualElementSwitcher
	{
		public void Enter(VisualElement from, VisualElement to)
		{
			IStyle lastElement = from.style;
			lastElement.display = DisplayStyle.None;
			lastElement.visibility = Visibility.Hidden;

			IStyle openedElement = to.style;
			openedElement.display = DisplayStyle.Flex;
			openedElement.visibility = Visibility.Visible;
		}

		public void Disable(VisualElement disabledElement) =>
			disabledElement.style.display = DisplayStyle.None;
	}
}