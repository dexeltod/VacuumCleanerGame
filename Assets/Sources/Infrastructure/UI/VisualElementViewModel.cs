using UnityEngine.UIElements;

namespace Infrastructure.UI
{
	public class VisualElementViewModel
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

		public void Disable(VisualElement disabledElement)
		{
			IStyle lastElement = disabledElement.style;
			lastElement.display = DisplayStyle.None;
		}
	}
}