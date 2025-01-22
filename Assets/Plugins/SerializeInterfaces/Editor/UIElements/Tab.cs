using UnityEngine.UIElements;

namespace Plugins.SerializeInterfaces.Editor.UIElements
{
	internal class Tab : Toggle
	{
		public Tab(string text)
		{
			this.text = text;
			RemoveFromClassList(
				ussClassName
			);
			AddToClassList(
				ussClassName
			);
		}
	}
}